USE EduFlex
GO
-- 1. Vai trò (không có phụ thuộc)
INSERT INTO [dbo].[Roles] ([RoleName], [Description], [CreatedAt]) VALUES
('Quản trị viên', N'Quản trị hệ thống với toàn quyền truy cập', '2024-01-01 00:00:00'),
('Giảng viên', N'Người dạy khóa học với quyền giảng dạy', '2024-01-01 00:00:00'),
('Học viên', N'Người dùng là học viên thông thường', '2024-01-01 00:00:00'),
('Người kiểm duyệt', N'Người kiểm duyệt nội dung có quyền xem xét', '2024-01-01 00:00:00'),
('Hỗ trợ', N'Nhân viên hỗ trợ khách hàng', '2024-01-01 00:00:00');

-- 2. Cấp độ khóa học (không có phụ thuộc)
INSERT INTO [dbo].[CourseLevels] ([LevelName], [DisplayOrder]) VALUES
(N'Người mới bắt đầu', 1),
(N'Trung cấp', 2),
(N'Nâng cao', 3),
(N'Chuyên gia', 4),
(N'Chuyên nghiệp', 5);

-- 3. Loại bài học (không có phụ thuộc)
INSERT INTO [dbo].[LessonTypes] ([TypeName]) VALUES
(N'Video'),
(N'Văn bản'),
(N'Bài kiểm tra'),
(N'Bài tập'),
(N'Buổi học trực tiếp');

-- 4. Người dùng (phụ thuộc vào vai trò)
INSERT INTO [dbo].[Users] ([Email], [PasswordHash], [FullName], [PhoneNumber], [Avatar], [Bio], [RoleId], [IsActive], [EmailVerified], [CreatedAt], [UpdatedAt], [LastLoginAt]) VALUES
('admin@example.com', 'hashed_password_1', N'John Admin', '+1234567890', '/uploads/avatars/default.jpg', N'Quản trị hệ thống', 1, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 10:00:00'),
('instructor1@example.com', 'hashed_password_2', N'Jane Smith', '+1234567891', '/uploads/avatars/default.jpg', N'Lập trình viên web có kinh nghiệm', 2, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 09:30:00'),
('student1@example.com', 'hashed_password_3', N'Bob Johnson', '+1234567892', '/uploads/avatars/default.jpg', N'Lập trình viên đang học hỏi', 3, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 11:15:00'),
('moderator@example.com', 'hashed_password_4', N'Alice Brown', '+1234567893', '/uploads/avatars/default.jpg', N'Người kiểm duyệt nội dung', 4, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 08:45:00'),
('support@example.com', 'hashed_password_5', N'Charlie Wilson', '+1234567894', '/uploads/avatars/default.jpg', N'Hỗ trợ khách hàng', 5, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 14:20:00');

-- 5. Danh mục (có phân cấp, thêm danh mục cha trước)
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Icon], [ParentCategoryId], [IsActive], [CreatedAt]) VALUES
(N'Lập trình', N'Khóa học phát triển phần mềm và lập trình', 'bi bi-code-slash', NULL, 1, '2024-01-01 00:00:00'),
(N'Thiết kế', N'Khóa học thiết kế UI/UX và đồ họa', 'bi bi-palette', NULL, 1, '2024-01-01 00:00:00'),
(N'Kinh doanh', N'Khóa học kinh doanh và khởi nghiệp', 'bi bi-briefcase', NULL, 1, '2024-01-01 00:00:00'),
(N'Phát triển Web', N'Phát triển web front-end và back-end', 'bi bi-window-stack', 1, 1, '2024-01-01 00:00:00'),
(N'Phát triển di động', N'Lập trình ứng dụng iOS và Android', 'bi bi-phone', 1, 1, '2024-01-01 00:00:00');

-- 6. Khóa học (phụ thuộc vào Người dùng, Danh mục, Cấp độ khóa học)
INSERT INTO [dbo].[Courses] ([CourseTitle], [Slug], [ShortDescription], [FullDescription], [ThumbnailUrl], [PreviewVideoUrl], [Price], [IsFree], [DiscountPrice], [InstructorId], [CategoryId], [LevelId], [Language], [Duration], [TotalLessons], [IsPublished], [IsApproved], [ApprovedBy], [ApprovedAt], [ViewCount], [EnrollmentCount], [AverageRating], [TotalRatings], [CreatedAt], [UpdatedAt]) VALUES
(N'Khóa học phát triển web toàn diện', 'complete-web-development-bootcamp', N'Học phát triển web full-stack từ đầu', N'Khóa học toàn diện bao gồm HTML, CSS, JavaScript, React, Node.js và MongoDB. Hoàn hảo cho người mới bắt đầu muốn trở thành lập trình viên web chuyên nghiệp.', 'web-dev-thumb.jpg', 'intro-video.mp4', 299.99, 0, 199.99, 2, 4, 1, N'Tiếng Anh', 120, 25, 1, 1, 1, '2024-01-01 00:00:00', 150, 45, 4.5, 12, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(N'Lập trình React nâng cao', 'advanced-react-development', N'Thành thạo React với hooks, context và các mẫu nâng cao', N'Khóa học chuyên sâu về hệ sinh thái React bao gồm hooks, context API, tối ưu hóa hiệu suất, kiểm thử và triển khai.', 'react-thumb.jpg', 'react-intro.mp4', 199.99, 0, 149.99, 2, 4, 3, N'Tiếng Anh', 80, 18, 1, 1, 1, '2024-01-01 00:00:00', 95, 28, 4.7, 8, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(N'Nguyên lý thiết kế UI/UX', 'ui-ux-design-fundamentals', N'Học nguyên tắc thiết kế và trải nghiệm người dùng', N'Khóa học toàn diện về thiết kế giao diện và trải nghiệm người dùng, bao gồm phác thảo, tạo nguyên mẫu và kiểm thử khả dụng.', 'ui-ux-thumb.jpg', 'design-intro.mp4', 179.99, 0, 129.99, 2, 2, 1, N'Tiếng Anh', 60, 15, 1, 1, 1, '2024-01-01 00:00:00', 75, 22, 4.3, 6, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(N'Python cho Khoa học Dữ liệu', 'python-data-science', N'Phân tích dữ liệu và học máy với Python', N'Học lập trình Python cho khoa học dữ liệu, bao gồm pandas, numpy, matplotlib và scikit-learn cho các ứng dụng học máy.', 'python-thumb.jpg', 'python-intro.mp4', 249.99, 0, 199.99, 2, 1, 2, N'Tiếng Anh', 100, 20, 1, 1, 1, '2024-01-01 00:00:00', 120, 35, 4.6, 10, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(N'Chiến lược Tiếp thị Số', 'digital-marketing-strategy', N'Khóa học tiếp thị kỹ thuật số toàn diện', N'Học SEO, tiếp thị mạng xã hội, tiếp thị qua email, chiến lược nội dung và phân tích để phát triển doanh nghiệp trực tuyến.', 'marketing-thumb.jpg', 'marketing-intro.mp4', 159.99, 0, 119.99, 2, 3, 2, N'Tiếng Anh', 70, 16, 1, 1, 1, '2024-01-01 00:00:00', 85, 30, 4.4, 7, '2024-01-01 00:00:00', '2024-01-01 00:00:00');

-- 7. Phần học (phụ thuộc vào Khóa học)
INSERT INTO [dbo].[Sections] ([CourseId], [SectionTitle], [Description], [DisplayOrder], [CreatedAt]) VALUES
(1, N'Giới thiệu về phát triển web', N'Bắt đầu với các kiến thức cơ bản về phát triển web', 1, '2024-01-01 00:00:00'),
(1, N'Cơ bản về HTML và CSS', N'Học các thành phần xây dựng của trang web', 2, '2024-01-01 00:00:00'),
(1, N'Lập trình JavaScript', N'Thành thạo JavaScript cho các trang web tương tác', 3, '2024-01-01 00:00:00'),
(2, N'Giới thiệu React', N'Giới thiệu các thành phần React và JSX', 1, '2024-01-01 00:00:00'),
(2, N'Mẫu React nâng cao', N'Hooks, context và tối ưu hóa hiệu suất', 2, '2024-01-01 00:00:00');

-- 8. Bài học (phụ thuộc vào Phần học, Loại bài học)
INSERT INTO [dbo].[Lessons] ([SectionId], [LessonTitle], [Description], [TypeId], [ContentUrl], [VideoUrl], [Duration], [IsFree], [DisplayOrder], [CreatedAt], [UpdatedAt]) VALUES
(1, N'Chào mừng đến với phát triển web', N'Giới thiệu khóa học và những gì bạn sẽ học', 1, '/content/welcome-lesson.html', 'welcome-video.mp4', 15, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(1, N'Cài đặt môi trường lập trình', N'Cài đặt và cấu hình các công cụ lập trình', 2, '/content/setup-guide.html', NULL, 20, 1, 2, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(2, N'Cấu trúc và thành phần HTML', N'Học các thẻ HTML và cấu trúc tài liệu', 1, '/content/html-basics.html', 'html-basics.mp4', 25, 0, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(2, N'Thiết kế và bố cục CSS', N'Trang trí HTML bằng CSS', 1, '/content/css-basics.html', 'css-basics.mp4', 30, 0, 2, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(3, N'Biến và hàm trong JavaScript', N'Giới thiệu lập trình JavaScript', 1, '/content/js-basics.html', 'js-basics.mp4', 35, 0, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00');

-- 9. Ghi danh khóa học (phụ thuộc vào Người dùng, Khóa học)
INSERT INTO [dbo].[Enrollments] ([UserId], [CourseId], [EnrolledAt], [CompletedAt], [Progress], [LastAccessedAt], [IsCertificateIssued]) VALUES
(3, 1, '2024-01-15 10:00:00', NULL, 25.5, '2024-01-20 14:30:00', 0),
(3, 2, '2024-01-20 14:30:00', NULL, 10.0, '2024-01-22 09:15:00', 0),
(4, 1, '2024-01-10 09:15:00', '2024-02-15 16:45:00', 100.0, '2024-02-15 16:45:00', 1),
(4, 3, '2024-01-25 11:20:00', NULL, 60.0, '2024-01-28 10:30:00', 0),
(5, 4, '2024-01-30 13:10:00', NULL, 40.0, '2024-02-02 15:20:00', 0);
-- 10. Bài kiểm tra (phụ thuộc vào Bài giảng)
INSERT INTO [dbo].[Quizzes] ([LessonId], [QuizTitle], [Description], [TimeLimit], [PassingScore], [MaxAttempts], [ShowCorrectAnswers], [CreatedAt]) VALUES
(1, N'Bài kiểm tra Cơ bản Phát triển Web', N'Kiểm tra mức độ hiểu biết của bạn về các kiến thức nền tảng phát triển web', 30, 70.00, 3, 1, '2024-01-01 00:00:00'),
(2, N'Bài kiểm tra Cài đặt Môi trường Phát triển', N'Xác minh rằng môi trường lập trình của bạn đã được cấu hình đúng cách', 15, 80.00, 2, 1, '2024-01-01 00:00:00'),
(3, N'Bài kiểm tra Kiến thức HTML Cơ bản', N'Kiểm tra kiến thức HTML của bạn', 25, 75.00, 3, 1, '2024-01-01 00:00:00'),
(4, N'Bài kiểm tra CSS Styling', N'Đánh giá kỹ năng CSS của bạn', 30, 70.00, 3, 1, '2024-01-01 00:00:00'),
(5, N'Bài kiểm tra JavaScript Cơ bản', N'Kiểm tra mức độ hiểu biết của bạn về JavaScript', 35, 80.00, 2, 1, '2024-01-01 00:00:00');

-- 11. Câu hỏi (phụ thuộc vào Bài kiểm tra)
INSERT INTO [dbo].[Questions] ([QuizId], [QuestionText], [QuestionType], [Points], [DisplayOrder], [Explanation], [CreatedAt]) VALUES
(1, N'HTML là viết tắt của gì?', N'Trắc nghiệm', 10, 1, N'HTML là viết tắt của HyperText Markup Language, ngôn ngữ đánh dấu tiêu chuẩn dùng để tạo trang web.', '2024-01-01 00:00:00'),
(1, N'Thẻ nào dùng để tạo liên kết (hyperlink)?', N'Trắc nghiệm', 10, 2, N'Thẻ <a> được sử dụng để tạo liên kết trong HTML. Nó cần thuộc tính href để chỉ định URL đích.', '2024-01-01 00:00:00'),
(1, N'Mục đích của CSS là gì?', N'Trắc nghiệm', 10, 3, N'CSS (Cascading Style Sheets) được dùng để định dạng và trang trí các phần tử HTML, kiểm soát giao diện trang web.', '2024-01-01 00:00:00'),
(2, N'Node.js được dùng để làm gì?', N'Trắc nghiệm', 15, 1, N'Node.js là môi trường chạy JavaScript phía máy chủ, cho phép lập trình backend bằng JavaScript.', '2024-01-01 00:00:00'),
(2, N'Lệnh nào dùng để cài đặt gói npm?', N'Trắc nghiệm', 15, 2, N'Lệnh npm install được dùng để cài đặt các gói từ kho npm vào dự án của bạn.', '2024-01-01 00:00:00');

-- 12. Câu trả lời (phụ thuộc vào Câu hỏi)
INSERT INTO [dbo].[Answers] ([QuestionId], [AnswerText], [IsCorrect], [DisplayOrder]) VALUES
(1, N'HyperText Markup Language', 1, 1),
(1, N'Home Tool Markup Language', 0, 2),
(1, N'Hyperlinks and Text Markup Language', 0, 3),
(2, N'<a>', 1, 1),
(2, N'<link>', 0, 2),
(2, N'<url>', 0, 3),
(3, N'Để định dạng giao diện trang web', 1, 1),
(3, N'Để tạo trang web', 0, 2),
(3, N'Để thêm tương tác', 0, 3),
(4, N'Môi trường chạy JavaScript phía máy chủ', 1, 1),
(4, N'Framework giao diện người dùng', 0, 2),
(4, N'Hệ thống quản lý cơ sở dữ liệu', 0, 3),
(5, N'npm install', 1, 1),
(5, N'npm add', 0, 2),
(5, N'npm get', 0, 3);

-- 13. Lượt làm bài kiểm tra (phụ thuộc vào Quizzes, Users)
INSERT INTO [dbo].[QuizAttempts] ([QuizId], [UserId], [Score], [TotalQuestions], [CorrectAnswers], [IsPassed], [StartedAt], [CompletedAt], [TimeSpent]) VALUES
(1, 3, 85.00, 3, 2, 1, '2024-01-16 10:30:00', '2024-01-16 10:45:00', 15),
(1, 3, 90.00, 3, 3, 1, '2024-01-17 14:20:00', '2024-01-17 14:35:00', 15),
(2, 3, 75.00, 2, 1, 0, '2024-01-18 09:15:00', '2024-01-18 09:25:00', 10),
(3, 4, 95.00, 3, 3, 1, '2024-01-20 11:00:00', '2024-01-20 11:20:00', 20),
(4, 4, 80.00, 3, 2, 1, '2024-01-22 15:30:00', '2024-01-22 15:50:00', 20);

-- 14. Câu trả lời của học viên (phụ thuộc vào QuizAttempts, Questions, Answers)
INSERT INTO [dbo].[StudentAnswers] ([AttemptId], [QuestionId], [AnswerId], [IsCorrect], [AnsweredAt]) VALUES
(1, 1, 1, 1, '2024-01-16 10:35:00'),
(1, 2, 4, 1, '2024-01-16 10:40:00'),
(1, 3, 6, 0, '2024-01-16 10:42:00'),
(2, 1, 1, 1, '2024-01-17 14:25:00'),
(2, 2, 4, 1, '2024-01-17 14:30:00'),
(2, 3, 7, 1, '2024-01-17 14:32:00'),
(3, 4, 10, 1, '2024-01-18 09:18:00'),
(3, 5, 12, 0, '2024-01-18 09:22:00'),
(4, 1, 1, 1, '2024-01-20 11:05:00'),
(4, 2, 4, 1, '2024-01-20 11:10:00'),
(4, 3, 7, 1, '2024-01-20 11:15:00'),
(5, 1, 1, 1, '2024-01-22 15:35:00'),
(5, 2, 4, 1, '2024-01-22 15:40:00'),
(5, 3, 6, 0, '2024-01-22 15:45:00');

-- 15. Mục tiêu khóa học (phụ thuộc vào Courses)
INSERT INTO [dbo].[CourseObjectives] ([CourseId], [Objective], [DisplayOrder]) VALUES
(1, N'Thành thạo các phần tử và cấu trúc HTML5 ngữ nghĩa', 1),
(1, N'Học các kỹ thuật CSS3 nâng cao', 2),
(1, N'Xây dựng trang web tương tác bằng JavaScript', 3),
(1, N'Tạo ứng dụng web đáp ứng (responsive)', 4),
(1, N'Triển khai ứng dụng web lên môi trường thực tế', 5),
(2, N'Hiểu vòng đời của component trong React', 1),
(2, N'Thành thạo hooks và quản lý trạng thái trong React', 2),
(2, N'Thực hiện điều hướng và routing', 3),
(2, N'Tối ưu hiệu suất ứng dụng React', 4),
(2, N'Thực hành kiểm thử component React hiệu quả', 5);

-- 16. Yêu cầu khóa học (phụ thuộc vào Courses)
INSERT INTO [dbo].[CourseRequirements] ([CourseId], [Requirement], [DisplayOrder]) VALUES
(1, N'Có kỹ năng máy tính cơ bản và kết nối Internet', 1),
(1, N'Cần có trình soạn thảo mã (đề xuất VS Code)', 2),
(1, N'Trình duyệt web hiện đại', 3),
(1, N'Không yêu cầu kinh nghiệm lập trình trước', 4),
(1, N'Cần sự kiên trì để hoàn thành khóa học', 5),
(2, N'Kiến thức cơ bản về HTML, CSS và JavaScript', 1),
(2, N'Đã cài đặt Node.js và npm', 2),
(2, N'Sử dụng hệ thống quản lý mã nguồn Git', 3),
(2, N'Hiểu các tính năng ES6+', 4),
(2, N'Có kinh nghiệm React trước là một lợi thế nhưng không bắt buộc', 5);

-- 17. Đánh giá khóa học (phụ thuộc vào Courses, Users)
INSERT INTO [dbo].[CourseReviews] ([CourseId], [UserId], [Rating], [ReviewText], [IsApproved], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, 5, N'Khóa học tuyệt vời! Giảng viên giải thích rất dễ hiểu và các dự án mang tính thực tế cao.', 1, '2024-01-25 10:00:00', '2024-01-25 10:00:00'),
(1, 4, 4, N'Nội dung hay và có cấu trúc rõ ràng. Rất phù hợp cho người mới bắt đầu học phát triển web.', 1, '2024-01-26 14:30:00', '2024-01-26 14:30:00'),
(2, 3, 5, N'Các khái niệm nâng cao được giải thích rất chi tiết. Bài tập thực hành giúp củng cố kiến thức hiệu quả.', 1, '2024-01-28 09:15:00', '2024-01-28 09:15:00'),
(2, 4, 4, N'Khóa học tốt cho các lập trình viên React muốn nâng cao kỹ năng.', 1, '2024-01-30 16:45:00', '2024-01-30 16:45:00'),
(3, 5, 5, N'Khóa học thiết kế tuyệt vời! Học được rất nhiều về các nguyên tắc UI/UX.', 1, '2024-02-01 11:20:00', '2024-02-01 11:20:00');
-- 18. CourseViews (phụ thuộc vào Courses, Users)
INSERT INTO [dbo].[CourseViews] ([CourseId], [UserId], [ViewedAt], [IpAddress], [UserAgent]) VALUES
(1, 3, '2024-01-15 10:00:00', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(1, 4, '2024-01-10 09:15:00', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(2, 3, '2024-01-20 14:30:00', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(2, 4, '2024-01-22 11:00:00', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(3, 5, '2024-01-25 11:20:00', '192.168.1.102', 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36');

-- 19. LessonProgress (phụ thuộc vào Enrollments, Lessons)
INSERT INTO [dbo].[LessonProgress] ([EnrollmentId], [LessonId], [IsCompleted], [CompletedAt], [LastWatchedPosition]) VALUES
(1, 1, 1, '2024-01-16 10:30:00', 100),
(1, 2, 1, '2024-01-17 14:20:00', 100),
(1, 3, 0, NULL, 30),
(2, 4, 1, '2024-01-21 11:00:00', 100),
(2, 5, 0, NULL, 15);

-- 20. LessonComments (phụ thuộc vào Lessons, Users)
INSERT INTO [dbo].[LessonComments] ([LessonId], [UserId], [ParentCommentId], [CommentText], [IsApproved], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, NULL, N'Bài giới thiệu rất tuyệt! Giải thích rất rõ ràng.', 1, '2024-01-16 10:35:00', '2024-01-16 10:35:00'),
(1, 4, NULL, N'Cảm ơn vì phần tổng quan. Mong chờ các phần tiếp theo của khóa học.', 1, '2024-01-16 11:00:00', '2024-01-16 11:00:00'),
(2, 3, NULL, N'Hướng dẫn cài đặt rất hữu ích. Mình đã làm theo và mọi thứ hoạt động tốt.', 1, '2024-01-17 14:25:00', '2024-01-17 14:25:00'),
(3, 4, NULL, N'Phần cơ bản về HTML được giải thích rất dễ hiểu. Ví dụ rất thực tế.', 1, '2024-01-20 11:05:00', '2024-01-20 11:05:00'),
(3, 3, 4, N'Mình đồng ý! Các ví dụ thực sự giúp hiểu rõ khái niệm hơn.', 1, '2024-01-20 11:30:00', '2024-01-20 11:30:00');

-- 21. LessonAttachments (phụ thuộc vào Lessons)
INSERT INTO [dbo].[LessonAttachments] ([LessonId], [FileName], [FileUrl], [FileSize], [FileType], [CreatedAt]) VALUES
(1, N'đề-cương-khóa-học.pdf', '/attachments/course-outline.pdf', 1024000, 'application/pdf', '2024-01-01 00:00:00'),
(1, N'danh-sách-tài-nguyên.docx', '/attachments/resources-list.docx', 512000, 'application/vnd.openxmlformats', '2024-01-01 00:00:00'),
(2, N'hướng-dẫn-cài-đặt.pdf', '/attachments/setup-guide.pdf', 2048000, 'application/pdf', '2024-01-01 00:00:00'),
(3, N'cheatsheet-html.pdf', '/attachments/html-cheatsheet.pdf', 768000, 'application/pdf', '2024-01-01 00:00:00'),
(4, N'ví-dụ-css.zip', '/attachments/css-examples.zip', 1536000, 'application/zip', '2024-01-01 00:00:00');

-- 22. CartItems (phụ thuộc vào Users, Courses)
INSERT INTO [dbo].[CartItems] ([UserId], [CourseId], [AddedAt]) VALUES
(3, 3, '2024-01-25 10:00:00'),
(3, 4, '2024-01-26 14:30:00'),
(4, 5, '2024-01-28 09:15:00'),
(5, 1, '2024-01-30 11:20:00'),
(5, 2, '2024-02-01 16:45:00');

-- 23. Orders (phụ thuộc vào Users)
INSERT INTO [dbo].[Orders] ([UserId], [OrderCode], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [PaymentStatus], [TransactionId], [CreatedAt], [PaidAt]) VALUES
(3, 'ORD-2024-001', 299.99, 100.00, 199.99, N'Thẻ tín dụng', N'Đã thanh toán', 'TXN-001-2024', '2024-01-15 10:00:00', '2024-01-15 10:05:00'),
(4, 'ORD-2024-002', 199.99, 50.00, 149.99, N'PayPal', N'Đã thanh toán', 'TXN-002-2024', '2024-01-20 14:30:00', '2024-01-20 14:32:00'),
(5, 'ORD-2024-003', 179.99, 50.00, 129.99, N'Thẻ tín dụng', N'Đang chờ xử lý', NULL, '2024-01-25 11:20:00', NULL),
(3, 'ORD-2024-004', 249.99, 0.00, 249.99, N'Chuyển khoản ngân hàng', N'Đang chờ xử lý', NULL, '2024-01-30 13:10:00', NULL),
(4, 'ORD-2024-005', 159.99, 40.00, 119.99, N'Thẻ tín dụng', N'Đã thanh toán', 'TXN-005-2024', '2024-02-01 16:45:00', '2024-02-01 16:47:00');

-- 24. OrderDetails (phụ thuộc vào Orders, Courses)
INSERT INTO [dbo].[OrderDetails] ([OrderId], [CourseId], [Price], [DiscountPrice]) VALUES
(1, 1, 299.99, 100.00),
(2, 2, 199.99, 50.00),
(3, 3, 179.99, 50.00),
(4, 4, 249.99, 0.00),
(5, 5, 159.99, 40.00);

-- 25. Coupons (không có phụ thuộc)
INSERT INTO [dbo].[Coupons] ([CouponCode], [Description], [DiscountType], [DiscountValue], [MinOrderAmount], [MaxDiscount], [UsageLimit], [UsedCount], [IsActive], [StartDate], [EndDate], [CreatedAt]) VALUES
('WELCOME10', N'Giảm giá chào mừng người dùng mới', 'Percentage', 10.00, 50.00, 50.00, 100, 25, 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', '2024-01-01 00:00:00'),
('SAVE20', N'Giảm 20% cho tất cả khóa học', 'Percentage', 20.00, 100.00, 100.00, 50, 15, 1, '2024-01-01 00:00:00', '2024-06-30 23:59:59', '2024-01-01 00:00:00'),
('FIXED50', N'Giảm cố định 50$', 'Fixed', 50.00, 200.00, 50.00, 25, 8, 1, '2024-01-01 00:00:00', '2024-03-31 23:59:59', '2024-01-01 00:00:00'),
('STUDENT15', N'Giảm giá cho sinh viên', 'Percentage', 15.00, 75.00, 75.00, 200, 45, 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', '2024-01-01 00:00:00'),
('EARLYBIRD', N'Ưu đãi đăng ký sớm', 'Percentage', 25.00, 150.00, 100.00, 30, 12, 1, '2024-01-01 00:00:00', '2024-02-29 23:59:59', '2024-01-01 00:00:00');
-- 26. Chứng chỉ (phụ thuộc vào bảng Enrollments)
INSERT INTO [dbo].[Certificates] ([EnrollmentId], [CertificateCode], [IssuedAt], [PdfUrl]) VALUES
(3, N'CERT-2024-001', '2024-02-15 16:45:00', N'/certificates/cert-2024-001.pdf'),
(1, N'CERT-2024-002', '2024-02-20 14:30:00', N'/certificates/cert-2024-002.pdf'),
(2, N'CERT-2024-003', '2024-02-25 11:00:00', N'/certificates/cert-2024-003.pdf'),
(4, N'CERT-2024-004', '2024-03-01 09:15:00', N'/certificates/cert-2024-004.pdf'),
(5, N'CERT-2024-005', '2024-03-05 16:20:00', N'/certificates/cert-2024-005.pdf');

-- 27. Hỏi & Đáp (phụ thuộc vào Courses, Users)
INSERT INTO [dbo].[QnA] ([CourseId], [UserId], [QuestionTitle], [QuestionText], [IsAnswered], [IsFeatured], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, N'Làm sao để gỡ lỗi JavaScript?', N'Tôi đang gặp lỗi cú pháp trong mã JavaScript của mình. Có ai có thể giúp tôi hiểu cách gỡ lỗi không?', 1, 0, '2024-01-18 10:30:00', '2024-01-18 10:30:00'),
(1, 4, N'So sánh CSS Grid và Flexbox', N'Khi nào tôi nên dùng CSS Grid thay vì Flexbox cho bố cục?', 1, 1, '2024-01-20 14:15:00', '2024-01-20 14:15:00'),
(2, 3, N'Thực hành tốt nhất với React Hooks', N'Những thực hành tốt nhất khi sử dụng React hooks trong functional components là gì?', 0, 0, '2024-01-22 09:45:00', '2024-01-22 09:45:00'),
(2, 4, N'Quản lý trạng thái trong React', N'Tôi nên dùng Context API hay Redux để quản lý trạng thái trong ứng dụng React lớn?', 1, 0, '2024-01-25 16:20:00', '2024-01-25 16:20:00'),
(3, 5, N'Thiết kế hệ thống component', N'Làm sao để tạo một hệ thống thiết kế nhất quán cho ứng dụng web của tôi?', 0, 0, '2024-01-28 11:10:00', '2024-01-28 11:10:00');

-- 28. Trả lời Hỏi & Đáp (phụ thuộc vào QnA, Users)
INSERT INTO [dbo].[QnAAnswers] ([QnAId], [UserId], [AnswerText], [IsAccepted], [Votes], [CreatedAt], [UpdatedAt]) VALUES
(1, 2, N'Sử dụng bảng điều khiển công cụ dành cho nhà phát triển của trình duyệt để kiểm tra lỗi JavaScript. Tìm thông báo lỗi màu đỏ và xem số dòng bị lỗi.', 1, 5, '2024-01-18 11:00:00', '2024-01-18 11:00:00'),
(2, 2, N'Dùng CSS Grid cho bố cục hai chiều (hàng và cột), và Flexbox cho bố cục một chiều (hàng hoặc cột).', 1, 8, '2024-01-20 15:30:00', '2024-01-20 15:30:00'),
(3, 2, N'Luôn sử dụng hook ở đầu component, không đặt trong vòng lặp hay điều kiện. Dùng useEffect cho các tác vụ phụ.', 0, 3, '2024-01-22 10:15:00', '2024-01-22 10:15:00'),
(4, 2, N'Với ứng dụng lớn, Redux giúp quản lý trạng thái tốt hơn với tính năng time-travel debugging. Context API phù hợp cho ứng dụng nhỏ.', 1, 6, '2024-01-25 17:45:00', '2024-01-25 17:45:00'),
(5, 2, N'Hãy bắt đầu với hệ thống thiết kế như Material-UI hoặc tạo thư viện component riêng với màu sắc, kiểu chữ và khoảng cách nhất quán.', 0, 2, '2024-01-28 12:30:00', '2024-01-28 12:30:00');

-- 29. Thông báo (phụ thuộc vào Users)
INSERT INTO [dbo].[Notifications] ([UserId], [Title], [Message], [Type], [IsRead], [Url], [CreatedAt]) VALUES
(3, N'Chào mừng đến với khóa học!', N'Bạn đã ghi danh thành công vào khóa "Lập trình Web toàn diện"', N'Enrollment', 1, N'/course/1', '2024-01-15 10:00:00'),
(3, N'Bài học mới có sẵn', N'Bài học 3: Lập trình JavaScript hiện đã được mở', N'Course Update', 0, N'/lesson/3', '2024-01-18 09:00:00'),
(4, N'Hoàn thành khóa học!', N'Chúc mừng! Bạn đã hoàn thành khóa "Lập trình Web toàn diện"', N'Achievement', 1, N'/certificate/1', '2024-02-15 16:45:00'),
(4, N'Chứng chỉ đã sẵn sàng', N'Chứng chỉ của bạn đã sẵn sàng để tải về', N'Certificate', 1, N'/certificate/download/1', '2024-02-15 17:00:00'),
(5, N'Nhắc nhở thanh toán', N'Thanh toán cho khóa học "Chiến lược Tiếp thị Số" của bạn vẫn đang chờ xử lý', N'Payment', 0, N'/payment/3', '2024-01-26 10:00:00');
-- 30. ActivityLogs (depends on Users)
INSERT INTO [dbo].[ActivityLogs] ([UserId], [Action], [EntityType], [EntityId], [Details], [IpAddress], [UserAgent], [CreatedAt]) VALUES
(3, N'Đăng nhập', N'Người dùng', 3, N'Người dùng đã đăng nhập thành công', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36', '2024-01-15 10:00:00'),
(3, N'Đăng ký học', N'Khóa học', 1, N'Đã ghi danh vào khóa học Lập trình Web Toàn diện', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36', '2024-01-15 10:05:00'),
(4, N'Hoàn thành', N'Bài học', 1, N'Đã hoàn thành bài học: Chào mừng đến với Lập trình Web', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36', '2024-01-16 10:30:00'),
(4, N'Đánh giá', N'Khóa học', 1, N'Đã đăng đánh giá cho khóa học Lập trình Web Toàn diện', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36', '2024-01-25 10:00:00'),
(5, N'Mua hàng', N'Đơn hàng', 3, N'Đã mua khóa học Chiến lược Tiếp thị Kỹ thuật số', '192.168.1.102', 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36', '2024-01-25 11:20:00');

-- 31. PasswordResetTokens (depends on Users)
INSERT INTO [dbo].[PasswordResetTokens] ([UserId], [Token], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES
(3, N'reset_token_123456789', '2024-01-16 10:00:00', 1, '2024-01-15 10:00:00'),
(4, N'reset_token_987654321', '2024-01-17 14:30:00', 0, '2024-01-16 14:30:00'),
(5, N'reset_token_456789123', '2024-01-18 09:15:00', 1, '2024-01-17 09:15:00'),
(3, N'reset_token_789123456', '2024-01-20 11:20:00', 0, '2024-01-19 11:20:00'),
(4, N'reset_token_321654987', '2024-01-22 16:45:00', 1, '2024-01-21 16:45:00');

-- 32. SystemAnnouncements (depends on Users)
INSERT INTO [dbo].[SystemAnnouncements] ([Title], [Content], [Type], [IsActive], [StartDate], [EndDate], [CreatedBy], [CreatedAt]) VALUES
(N'Chào mừng đến với Nền tảng Học tập của chúng tôi!', 
 N'Chúng tôi rất vui mừng khi bạn gia nhập cộng đồng học viên. Hãy bắt đầu hành trình học tập với các khóa học toàn diện của chúng tôi.', 
 N'Thông báo chung', 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', 1, '2024-01-01 00:00:00'),

(N'Khóa học mới: Lập trình React Nâng cao', 
 N'Khám phá khóa học mới nhất về các khái niệm React nâng cao bao gồm hooks, context và tối ưu hiệu năng.', 
 N'Khóa học', 1, '2024-01-15 00:00:00', '2024-12-31 23:59:59', 1, '2024-01-15 00:00:00'),

(N'Lịch bảo trì hệ thống', 
 N'Hệ thống sẽ được bảo trì vào Chủ Nhật, ngày 4 tháng 2 từ 2 giờ sáng đến 4 giờ sáng (giờ EST).', 
 N'Bảo trì', 1, '2024-01-30 00:00:00', '2024-02-04 23:59:59', 1, '2024-01-30 00:00:00'),

(N'Câu chuyện thành công của học viên', 
 N'Đọc về cách học viên của chúng tôi đã thay đổi sự nghiệp của họ thông qua các khóa học. Xem phần câu chuyện thành công.', 
 N'Thành công', 1, '2024-02-01 00:00:00', '2024-12-31 23:59:59', 1, '2024-02-01 00:00:00'),

(N'Cập nhật ứng dụng di động', 
 N'Tải xuống phiên bản mới nhất của ứng dụng di động để cải thiện hiệu suất và thêm các tính năng mới.', 
 N'Ứng dụng', 1, '2024-02-05 00:00:00', '2024-12-31 23:59:59', 1, '2024-02-05 00:00:00');

-- 33. AdminMenu (no dependencies)
INSERT INTO [dbo].[AdminMenu] ([ItemName], [ItemLevel], [ParentLevel], [ItemOrder], [IsActive], [ItemTarget], [AreaName], [ControllerName], [ActionName], [Icon], [IdName], [CreatedAt], [MenuType]) VALUES
(N'Tổng quan', 1, 0, 1, 1, '_self', 'Admin', 'Dashboard', 'Index', '/uploads/categories/default.jpg', 'dashboard', '2024-01-01 00:00:00', N'Chính'),
(N'Người dùng', 1, 0, 2, 1, '_self', 'Admin', 'Users', 'Users', '/uploads/categories/default.jpg', 'users', '2024-01-01 00:00:00', N'Chính'),
(N'Khóa học', 1, 0, 3, 1, '_self', 'Admin', 'Courses', 'Index', '/uploads/categories/default.jpg', 'courses', '2024-01-01 00:00:00', N'Chính'),
(N'Đơn hàng', 1, 0, 4, 1, '_self', 'Admin', 'Orders', 'Index', '/uploads/categories/default.jpg', 'orders', '2024-01-01 00:00:00', N'Chính'),
(N'Báo cáo', 1, 0, 5, 1, '_self', 'Admin', 'Reports', 'Index', '/uploads/categories/default.jpg', 'reports', '2024-01-01 00:00:00', N'Chính'),
(N'Quản lý người dùng', 2, 2, 1, 1, '_self', 'Admin', 'Users', 'Index', '/uploads/categories/default.jpg', 'user-mgmt', '2024-01-01 00:00:00', N'Phụ'),
(N'Quản lý vai trò', 2, 2, 2, 1, '_self', 'Admin', 'Roles', 'Index', '/uploads/categories/default.jpg', 'role-mgmt', '2024-01-01 00:00:00', N'Phụ'),
(N'Quản lý khóa học', 2, 3, 1, 1, '_self', 'Admin', 'Courses', 'Index', '/uploads/categories/default.jpg', 'course-mgmt', '2024-01-01 00:00:00', N'Phụ'),
(N'Quản lý danh mục', 2, 3, 2, 1, '_self', 'Admin', 'Categories', 'Index', '/uploads/categories/default.jpg', 'category-mgmt', '2024-01-01 00:00:00', N'Phụ'),
(N'Quản lý đơn hàng', 2, 4, 1, 1, '_self', 'Admin', 'Orders', 'Index', '/uploads/categories/default.jpg', 'order-mgmt', '2024-01-01 00:00:00', N'Phụ');
