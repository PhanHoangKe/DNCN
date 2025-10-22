-- Script để xóa an toàn 3 bản ghi không thể xóa được
-- Thực hiện theo thứ tự từ bản ghi con đến bản ghi cha

USE EduFlex
GO

-- =============================================
-- 1. XÓA CATEGORY "Programming" (ID = 1)
-- =============================================

-- Bước 1: Cập nhật các category con để không phụ thuộc vào category cha
UPDATE [dbo].[Categories] 
SET [ParentCategoryId] = NULL 
WHERE [ParentCategoryId] = 1;

-- Bước 2: Cập nhật các course thuộc category này sang category khác (nếu có)
-- Kiểm tra xem có course nào thuộc category 1 không
IF EXISTS (SELECT 1 FROM [dbo].[Courses] WHERE [CategoryId] = 1)
BEGIN
    -- Cập nhật các course sang category 2 (Design) hoặc 3 (Business)
    UPDATE [dbo].[Courses] 
    SET [CategoryId] = 2 
    WHERE [CategoryId] = 1;
END

-- Bước 3: Xóa category "Programming"
DELETE FROM [dbo].[Categories] 
WHERE [CategoryId] = 1;

PRINT 'Đã xóa category "Programming" (ID = 1) thành công';

-- =============================================
-- 2. XÓA USER "admin@example.com" (ID = 1)
-- =============================================

-- Bước 1: Xóa các bản ghi phụ thuộc trực tiếp
-- Xóa SystemAnnouncements được tạo bởi user này
DELETE FROM [dbo].[SystemAnnouncements] 
WHERE [CreatedBy] = 1;

-- Xóa ActivityLogs của user này
DELETE FROM [dbo].[ActivityLogs] 
WHERE [UserId] = 1;

-- Xóa Notifications của user này
DELETE FROM [dbo].[Notifications] 
WHERE [UserId] = 1;

-- Xóa PasswordResetTokens của user này
DELETE FROM [dbo].[PasswordResetTokens] 
WHERE [UserId] = 1;

-- Xóa CartItems của user này
DELETE FROM [dbo].[CartItems] 
WHERE [UserId] = 1;

-- Xóa CourseReviews của user này
DELETE FROM [dbo].[CourseReviews] 
WHERE [UserId] = 1;

-- Xóa CourseViews của user này
DELETE FROM [dbo].[CourseViews] 
WHERE [UserId] = 1;

-- Xóa QnA của user này
DELETE FROM [dbo].[QnA] 
WHERE [UserId] = 1;

-- Xóa QnAAnswers của user này
DELETE FROM [dbo].[QnAAnswers] 
WHERE [UserId] = 1;

-- Xóa LessonComments của user này
DELETE FROM [dbo].[LessonComments] 
WHERE [UserId] = 1;

-- Bước 2: Cập nhật các bản ghi có tham chiếu đến user này
-- Cập nhật Courses được phê duyệt bởi user này
UPDATE [dbo].[Courses] 
SET [ApprovedBy] = 2  -- Chuyển sang instructor khác
WHERE [ApprovedBy] = 1;

-- Cập nhật Enrollments của user này (nếu có)
DELETE FROM [dbo].[Enrollments] 
WHERE [UserId] = 1;

-- Xóa LessonProgress liên quan đến enrollments đã xóa
DELETE FROM [dbo].[LessonProgress] 
WHERE [EnrollmentId] IN (
    SELECT [EnrollmentId] FROM [dbo].[Enrollments] WHERE [UserId] = 1
);

-- Xóa Certificates liên quan đến enrollments đã xóa
DELETE FROM [dbo].[Certificates] 
WHERE [EnrollmentId] IN (
    SELECT [EnrollmentId] FROM [dbo].[Enrollments] WHERE [UserId] = 1
);

-- Bước 3: Xóa user "admin@example.com"
DELETE FROM [dbo].[Users] 
WHERE [UserId] = 1;

PRINT 'Đã xóa user "admin@example.com" (ID = 1) thành công';

-- =============================================
-- 3. XÓA ROLE "Admin" (ID = 1)
-- =============================================

-- Bước 1: Cập nhật tất cả users có RoleId = 1 sang role khác
UPDATE [dbo].[Users] 
SET [RoleId] = 2  -- Chuyển sang role "Instructor"
WHERE [RoleId] = 1;

-- Bước 2: Xóa role "Admin"
DELETE FROM [dbo].[Roles] 
WHERE [RoleId] = 1;

PRINT 'Đã xóa role "Admin" (ID = 1) thành công';

-- =============================================
-- KIỂM TRA KẾT QUẢ
-- =============================================

-- Kiểm tra xem các bản ghi đã được xóa chưa
SELECT 'Categories' as TableName, COUNT(*) as RemainingRecords 
FROM [dbo].[Categories] 
WHERE [CategoryId] = 1
UNION ALL
SELECT 'Users', COUNT(*) 
FROM [dbo].[Users] 
WHERE [UserId] = 1
UNION ALL
SELECT 'Roles', COUNT(*) 
FROM [dbo].[Roles] 
WHERE [RoleId] = 1;

PRINT 'Script xóa hoàn tất!';
PRINT 'Các bản ghi đã được xóa theo thứ tự an toàn.';