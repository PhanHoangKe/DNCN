USE EduFlex
GO
-- 1. Roles (no dependencies)
INSERT INTO [dbo].[Roles] ([RoleName], [Description], [CreatedAt]) VALUES
('Admin', 'System Administrator with full access', '2024-01-01 00:00:00'),
('Instructor', 'Course instructor with teaching privileges', '2024-01-01 00:00:00'),
('Student', 'Regular student user', '2024-01-01 00:00:00'),
('Moderator', 'Content moderator with review privileges', '2024-01-01 00:00:00'),
('Support', 'Customer support representative', '2024-01-01 00:00:00');

-- 2. CourseLevels (no dependencies)
INSERT INTO [dbo].[CourseLevels] ([LevelName], [DisplayOrder]) VALUES
('Beginner', 1),
('Intermediate', 2),
('Advanced', 3),
('Expert', 4),
('Professional', 5);

-- 3. LessonTypes (no dependencies)
INSERT INTO [dbo].[LessonTypes] ([TypeName]) VALUES
('Video'),
('Text'),
('Quiz'),
('Assignment'),
('Live Session');

-- 4. Users (depends on Roles)
INSERT INTO [dbo].[Users] ([Email], [PasswordHash], [FullName], [PhoneNumber], [Avatar], [Bio], [RoleId], [IsActive], [EmailVerified], [CreatedAt], [UpdatedAt], [LastLoginAt]) VALUES
('admin@example.com', 'hashed_password_1', 'John Admin', '+1234567890', '/uploads/avatars/default.jpg', 'System Administrator', 1, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 10:00:00'),
('instructor1@example.com', 'hashed_password_2', 'Jane Smith', '+1234567891', '/uploads/avatars/default.jpg', 'Experienced Web Developer', 2, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 09:30:00'),
('student1@example.com', 'hashed_password_3', 'Bob Johnson', '+1234567892', '/uploads/avatars/default.jpg', 'Aspiring Developer', 3, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 11:15:00'),
('moderator@example.com', 'hashed_password_4', 'Alice Brown', '+1234567893', '/uploads/avatars/default.jpg', 'Content Moderator', 4, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 08:45:00'),
('support@example.com', 'hashed_password_5', 'Charlie Wilson', '+1234567894', '/uploads/avatars/default.jpg', 'Customer Support', 5, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00', '2024-01-15 14:20:00');

-- 5. Categories (self-referencing, insert parent categories first)
INSERT INTO [dbo].[Categories] ([CategoryName], [Description], [Icon], [ParentCategoryId], [IsActive], [CreatedAt]) VALUES
('Programming', 'Software development and coding courses', 'bi bi-code-slash', NULL, 1, '2024-01-01 00:00:00'),
('Design', 'UI/UX and graphic design courses', 'bi bi-palette', NULL, 1, '2024-01-01 00:00:00'),
('Business', 'Business and entrepreneurship courses', 'bi bi-briefcase', NULL, 1, '2024-01-01 00:00:00'),
('Web Development', 'Frontend and backend web development', 'bi bi-window-stack', 1, 1, '2024-01-01 00:00:00'),
('Mobile Development', 'iOS and Android app development', 'bi bi-phone', 1, 1, '2024-01-01 00:00:00');

-- 6. Courses (depends on Users, Categories, CourseLevels)
INSERT INTO [dbo].[Courses] ([CourseTitle], [Slug], [ShortDescription], [FullDescription], [ThumbnailUrl], [PreviewVideoUrl], [Price], [IsFree], [DiscountPrice], [InstructorId], [CategoryId], [LevelId], [Language], [Duration], [TotalLessons], [IsPublished], [IsApproved], [ApprovedBy], [ApprovedAt], [ViewCount], [EnrollmentCount], [AverageRating], [TotalRatings], [CreatedAt], [UpdatedAt]) VALUES
('Complete Web Development Bootcamp', 'complete-web-development-bootcamp', 'Learn full-stack web development from scratch', 'A comprehensive course covering HTML, CSS, JavaScript, React, Node.js, and MongoDB. Perfect for beginners who want to become professional web developers.', 'web-dev-thumb.jpg', 'intro-video.mp4', 299.99, 0, 199.99, 2, 4, 1, 'English', 120, 25, 1, 1, 1, '2024-01-01 00:00:00', 150, 45, 4.5, 12, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
('Advanced React Development', 'advanced-react-development', 'Master React with hooks, context, and advanced patterns', 'Deep dive into React ecosystem including hooks, context API, performance optimization, testing, and deployment strategies.', 'react-thumb.jpg', 'react-intro.mp4', 199.99, 0, 149.99, 2, 4, 3, 'English', 80, 18, 1, 1, 1, '2024-01-01 00:00:00', 95, 28, 4.7, 8, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
('UI/UX Design Fundamentals', 'ui-ux-design-fundamentals', 'Learn design principles and user experience', 'Comprehensive course on user interface and user experience design, including wireframing, prototyping, and usability testing.', 'ui-ux-thumb.jpg', 'design-intro.mp4', 179.99, 0, 129.99, 2, 2, 1, 'English', 60, 15, 1, 1, 1, '2024-01-01 00:00:00', 75, 22, 4.3, 6, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
('Python for Data Science', 'python-data-science', 'Data analysis and machine learning with Python', 'Learn Python programming for data science, including pandas, numpy, matplotlib, and scikit-learn for machine learning applications.', 'python-thumb.jpg', 'python-intro.mp4', 249.99, 0, 199.99, 2, 1, 2, 'English', 100, 20, 1, 1, 1, '2024-01-01 00:00:00', 120, 35, 4.6, 10, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
('Digital Marketing Strategy', 'digital-marketing-strategy', 'Complete digital marketing course', 'Learn SEO, social media marketing, email marketing, content strategy, and analytics to grow your business online.', 'marketing-thumb.jpg', 'marketing-intro.mp4', 159.99, 0, 119.99, 2, 3, 2, 'English', 70, 16, 1, 1, 1, '2024-01-01 00:00:00', 85, 30, 4.4, 7, '2024-01-01 00:00:00', '2024-01-01 00:00:00');

-- 7. Sections (depends on Courses)
INSERT INTO [dbo].[Sections] ([CourseId], [SectionTitle], [Description], [DisplayOrder], [CreatedAt]) VALUES
(1, 'Introduction to Web Development', 'Get started with the basics of web development', 1, '2024-01-01 00:00:00'),
(1, 'HTML and CSS Fundamentals', 'Learn the building blocks of web pages', 2, '2024-01-01 00:00:00'),
(1, 'JavaScript Programming', 'Master JavaScript for interactive websites', 3, '2024-01-01 00:00:00'),
(2, 'React Basics', 'Introduction to React components and JSX', 1, '2024-01-01 00:00:00'),
(2, 'Advanced React Patterns', 'Hooks, context, and performance optimization', 2, '2024-01-01 00:00:00');

-- 8. Lessons (depends on Sections, LessonTypes)
INSERT INTO [dbo].[Lessons] ([SectionId], [LessonTitle], [Description], [TypeId], [ContentUrl], [VideoUrl], [Duration], [IsFree], [DisplayOrder], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Welcome to Web Development', 'Introduction to the course and what you will learn', 1, '/content/welcome-lesson.html', 'welcome-video.mp4', 15, 1, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(1, 'Setting Up Your Development Environment', 'Install and configure your coding tools', 2, '/content/setup-guide.html', NULL, 20, 1, 2, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(2, 'HTML Structure and Elements', 'Learn HTML tags and document structure', 1, '/content/html-basics.html', 'html-basics.mp4', 25, 0, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(2, 'CSS Styling and Layout', 'Style your HTML with CSS', 1, '/content/css-basics.html', 'css-basics.mp4', 30, 0, 2, '2024-01-01 00:00:00', '2024-01-01 00:00:00'),
(3, 'JavaScript Variables and Functions', 'Introduction to JavaScript programming', 1, '/content/js-basics.html', 'js-basics.mp4', 35, 0, 1, '2024-01-01 00:00:00', '2024-01-01 00:00:00');

-- 9. Enrollments (depends on Users, Courses)
INSERT INTO [dbo].[Enrollments] ([UserId], [CourseId], [EnrolledAt], [CompletedAt], [Progress], [LastAccessedAt], [IsCertificateIssued]) VALUES
(3, 1, '2024-01-15 10:00:00', NULL, 25.5, '2024-01-20 14:30:00', 0),
(3, 2, '2024-01-20 14:30:00', NULL, 10.0, '2024-01-22 09:15:00', 0),
(4, 1, '2024-01-10 09:15:00', '2024-02-15 16:45:00', 100.0, '2024-02-15 16:45:00', 1),
(4, 3, '2024-01-25 11:20:00', NULL, 60.0, '2024-01-28 10:30:00', 0),
(5, 4, '2024-01-30 13:10:00', NULL, 40.0, '2024-02-02 15:20:00', 0);

-- 10. Quizzes (depends on Lessons)
INSERT INTO [dbo].[Quizzes] ([LessonId], [QuizTitle], [Description], [TimeLimit], [PassingScore], [MaxAttempts], [ShowCorrectAnswers], [CreatedAt]) VALUES
(1, 'Web Development Basics Quiz', 'Test your understanding of web development fundamentals', 30, 70.00, 3, 1, '2024-01-01 00:00:00'),
(2, 'Development Environment Setup Quiz', 'Verify your development environment is properly configured', 15, 80.00, 2, 1, '2024-01-01 00:00:00'),
(3, 'HTML Fundamentals Quiz', 'Test your HTML knowledge', 25, 75.00, 3, 1, '2024-01-01 00:00:00'),
(4, 'CSS Styling Quiz', 'Evaluate your CSS skills', 30, 70.00, 3, 1, '2024-01-01 00:00:00'),
(5, 'JavaScript Basics Quiz', 'Test your JavaScript understanding', 35, 80.00, 2, 1, '2024-01-01 00:00:00');

-- 11. Questions (depends on Quizzes)
INSERT INTO [dbo].[Questions] ([QuizId], [QuestionText], [QuestionType], [Points], [DisplayOrder], [Explanation], [CreatedAt]) VALUES
(1, 'What does HTML stand for?', 'Multiple Choice', 10, 1, 'HTML stands for HyperText Markup Language, which is the standard markup language for creating web pages.', '2024-01-01 00:00:00'),
(1, 'Which tag is used to create a hyperlink?', 'Multiple Choice', 10, 2, 'The <a> tag is used to create hyperlinks in HTML. It requires an href attribute to specify the destination URL.', '2024-01-01 00:00:00'),
(1, 'What is the purpose of CSS?', 'Multiple Choice', 10, 3, 'CSS (Cascading Style Sheets) is used to style and format HTML elements, controlling the appearance of web pages.', '2024-01-01 00:00:00'),
(2, 'What is Node.js used for?', 'Multiple Choice', 15, 1, 'Node.js is a JavaScript runtime that allows you to run JavaScript on the server side, enabling backend development.', '2024-01-01 00:00:00'),
(2, 'Which command installs npm packages?', 'Multiple Choice', 15, 2, 'The npm install command is used to install packages from the npm registry into your project.', '2024-01-01 00:00:00');

-- 12. Answers (depends on Questions)
INSERT INTO [dbo].[Answers] ([QuestionId], [AnswerText], [IsCorrect], [DisplayOrder]) VALUES
(1, 'HyperText Markup Language', 1, 1),
(1, 'Home Tool Markup Language', 0, 2),
(1, 'Hyperlinks and Text Markup Language', 0, 3),
(2, '<a>', 1, 1),
(2, '<link>', 0, 2),
(2, '<url>', 0, 3),
(3, 'To style web pages', 1, 1),
(3, 'To create web pages', 0, 2),
(3, 'To add interactivity', 0, 3),
(4, 'Server-side JavaScript runtime', 1, 1),
(4, 'Frontend framework', 0, 2),
(4, 'Database management system', 0, 3),
(5, 'npm install', 1, 1),
(5, 'npm add', 0, 2),
(5, 'npm get', 0, 3);

-- 13. QuizAttempts (depends on Quizzes, Users)
INSERT INTO [dbo].[QuizAttempts] ([QuizId], [UserId], [Score], [TotalQuestions], [CorrectAnswers], [IsPassed], [StartedAt], [CompletedAt], [TimeSpent]) VALUES
(1, 3, 85.00, 3, 2, 1, '2024-01-16 10:30:00', '2024-01-16 10:45:00', 15),
(1, 3, 90.00, 3, 3, 1, '2024-01-17 14:20:00', '2024-01-17 14:35:00', 15),
(2, 3, 75.00, 2, 1, 0, '2024-01-18 09:15:00', '2024-01-18 09:25:00', 10),
(3, 4, 95.00, 3, 3, 1, '2024-01-20 11:00:00', '2024-01-20 11:20:00', 20),
(4, 4, 80.00, 3, 2, 1, '2024-01-22 15:30:00', '2024-01-22 15:50:00', 20);

-- 14. StudentAnswers (depends on QuizAttempts, Questions, Answers)
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

-- 15. CourseObjectives (depends on Courses)
INSERT INTO [dbo].[CourseObjectives] ([CourseId], [Objective], [DisplayOrder]) VALUES
(1, 'Master HTML5 semantic elements and structure', 1),
(1, 'Learn CSS3 advanced styling techniques', 2),
(1, 'Build interactive websites with JavaScript', 3),
(1, 'Create responsive web applications', 4),
(1, 'Deploy web applications to production', 5),
(2, 'Understand React component lifecycle', 1),
(2, 'Master React hooks and state management', 2),
(2, 'Implement routing and navigation', 3),
(2, 'Optimize React application performance', 4),
(2, 'Test React components effectively', 5);

-- 16. CourseRequirements (depends on Courses)
INSERT INTO [dbo].[CourseRequirements] ([CourseId], [Requirement], [DisplayOrder]) VALUES
(1, 'Basic computer skills and internet access', 1),
(1, 'Text editor (VS Code recommended)', 2),
(1, 'Modern web browser', 3),
(1, 'No prior programming experience required', 4),
(1, 'Dedication to complete the course', 5),
(2, 'Basic knowledge of HTML, CSS, and JavaScript', 1),
(2, 'Node.js and npm installed', 2),
(2, 'Git version control', 3),
(2, 'Understanding of ES6+ features', 4),
(2, 'Previous React experience helpful but not required', 5);

-- 17. CourseReviews (depends on Courses, Users)
INSERT INTO [dbo].[CourseReviews] ([CourseId], [UserId], [Rating], [ReviewText], [IsApproved], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, 5, 'Excellent course! The instructor explains everything clearly and the projects are very practical.', 1, '2024-01-25 10:00:00', '2024-01-25 10:00:00'),
(1, 4, 4, 'Great content and well-structured. Would recommend to anyone starting web development.', 1, '2024-01-26 14:30:00', '2024-01-26 14:30:00'),
(2, 3, 5, 'Advanced concepts explained perfectly. The hands-on projects really help solidify learning.', 1, '2024-01-28 09:15:00', '2024-01-28 09:15:00'),
(2, 4, 4, 'Good course for React developers looking to level up their skills.', 1, '2024-01-30 16:45:00', '2024-01-30 16:45:00'),
(3, 5, 5, 'Amazing design course! Learned so much about UI/UX principles.', 1, '2024-02-01 11:20:00', '2024-02-01 11:20:00');

-- 18. CourseViews (depends on Courses, Users)
INSERT INTO [dbo].[CourseViews] ([CourseId], [UserId], [ViewedAt], [IpAddress], [UserAgent]) VALUES
(1, 3, '2024-01-15 10:00:00', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(1, 4, '2024-01-10 09:15:00', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(2, 3, '2024-01-20 14:30:00', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'),
(2, 4, '2024-01-22 11:00:00', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36'),
(3, 5, '2024-01-25 11:20:00', '192.168.1.102', 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36');

-- 19. LessonProgress (depends on Enrollments, Lessons)
INSERT INTO [dbo].[LessonProgress] ([EnrollmentId], [LessonId], [IsCompleted], [CompletedAt], [LastWatchedPosition]) VALUES
(1, 1, 1, '2024-01-16 10:30:00', 100),
(1, 2, 1, '2024-01-17 14:20:00', 100),
(1, 3, 0, NULL, 30),
(2, 4, 1, '2024-01-21 11:00:00', 100),
(2, 5, 0, NULL, 15);

-- 20. LessonComments (depends on Lessons, Users)
INSERT INTO [dbo].[LessonComments] ([LessonId], [UserId], [ParentCommentId], [CommentText], [IsApproved], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, NULL, 'Great introduction! Very clear explanation.', 1, '2024-01-16 10:35:00', '2024-01-16 10:35:00'),
(1, 4, NULL, 'Thanks for the overview. Looking forward to the rest of the course.', 1, '2024-01-16 11:00:00', '2024-01-16 11:00:00'),
(2, 3, NULL, 'The setup instructions were very helpful. Got everything working quickly.', 1, '2024-01-17 14:25:00', '2024-01-17 14:25:00'),
(3, 4, NULL, 'HTML basics are well explained. The examples are practical.', 1, '2024-01-20 11:05:00', '2024-01-20 11:05:00'),
(3, 3, 4, 'I agree! The examples really help understand the concepts.', 1, '2024-01-20 11:30:00', '2024-01-20 11:30:00');

-- 21. LessonAttachments (depends on Lessons)
INSERT INTO [dbo].[LessonAttachments] ([LessonId], [FileName], [FileUrl], [FileSize], [FileType], [CreatedAt]) VALUES
(1, 'course-outline.pdf', '/attachments/course-outline.pdf', 1024000, 'application/pdf', '2024-01-01 00:00:00'),
(1, 'resources-list.docx', '/attachments/resources-list.docx', 512000, 'application/vnd.openxmlformats', '2024-01-01 00:00:00'),
(2, 'setup-guide.pdf', '/attachments/setup-guide.pdf', 2048000, 'application/pdf', '2024-01-01 00:00:00'),
(3, 'html-cheatsheet.pdf', '/attachments/html-cheatsheet.pdf', 768000, 'application/pdf', '2024-01-01 00:00:00'),
(4, 'css-examples.zip', '/attachments/css-examples.zip', 1536000, 'application/zip', '2024-01-01 00:00:00');

-- 22. CartItems (depends on Users, Courses)
INSERT INTO [dbo].[CartItems] ([UserId], [CourseId], [AddedAt]) VALUES
(3, 3, '2024-01-25 10:00:00'),
(3, 4, '2024-01-26 14:30:00'),
(4, 5, '2024-01-28 09:15:00'),
(5, 1, '2024-01-30 11:20:00'),
(5, 2, '2024-02-01 16:45:00');

-- 23. Orders (depends on Users)
INSERT INTO [dbo].[Orders] ([UserId], [OrderCode], [TotalAmount], [DiscountAmount], [FinalAmount], [PaymentMethod], [PaymentStatus], [TransactionId], [CreatedAt], [PaidAt]) VALUES
(3, 'ORD-2024-001', 299.99, 100.00, 199.99, 'Credit Card', 'Paid', 'TXN-001-2024', '2024-01-15 10:00:00', '2024-01-15 10:05:00'),
(4, 'ORD-2024-002', 199.99, 50.00, 149.99, 'PayPal', 'Paid', 'TXN-002-2024', '2024-01-20 14:30:00', '2024-01-20 14:32:00'),
(5, 'ORD-2024-003', 179.99, 50.00, 129.99, 'Credit Card', 'Pending', NULL, '2024-01-25 11:20:00', NULL),
(3, 'ORD-2024-004', 249.99, 0.00, 249.99, 'Bank Transfer', 'Pending', NULL, '2024-01-30 13:10:00', NULL),
(4, 'ORD-2024-005', 159.99, 40.00, 119.99, 'Credit Card', 'Paid', 'TXN-005-2024', '2024-02-01 16:45:00', '2024-02-01 16:47:00');

-- 24. OrderDetails (depends on Orders, Courses)
INSERT INTO [dbo].[OrderDetails] ([OrderId], [CourseId], [Price], [DiscountPrice]) VALUES
(1, 1, 299.99, 100.00),
(2, 2, 199.99, 50.00),
(3, 3, 179.99, 50.00),
(4, 4, 249.99, 0.00),
(5, 5, 159.99, 40.00);

-- 25. Coupons (no dependencies)
INSERT INTO [dbo].[Coupons] ([CouponCode], [Description], [DiscountType], [DiscountValue], [MinOrderAmount], [MaxDiscount], [UsageLimit], [UsedCount], [IsActive], [StartDate], [EndDate], [CreatedAt]) VALUES
('WELCOME10', 'Welcome discount for new users', 'Percentage', 10.00, 50.00, 50.00, 100, 25, 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', '2024-01-01 00:00:00'),
('SAVE20', '20% off on all courses', 'Percentage', 20.00, 100.00, 100.00, 50, 15, 1, '2024-01-01 00:00:00', '2024-06-30 23:59:59', '2024-01-01 00:00:00'),
('FIXED50', 'Fixed $50 discount', 'Fixed', 50.00, 200.00, 50.00, 25, 8, 1, '2024-01-01 00:00:00', '2024-03-31 23:59:59', '2024-01-01 00:00:00'),
('STUDENT15', 'Student discount', 'Percentage', 15.00, 75.00, 75.00, 200, 45, 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', '2024-01-01 00:00:00'),
('EARLYBIRD', 'Early bird special', 'Percentage', 25.00, 150.00, 100.00, 30, 12, 1, '2024-01-01 00:00:00', '2024-02-29 23:59:59', '2024-01-01 00:00:00');

-- 26. Certificates (depends on Enrollments)
INSERT INTO [dbo].[Certificates] ([EnrollmentId], [CertificateCode], [IssuedAt], [PdfUrl]) VALUES
(3, 'CERT-2024-001', '2024-02-15 16:45:00', '/certificates/cert-2024-001.pdf'),
(1, 'CERT-2024-002', '2024-02-20 14:30:00', '/certificates/cert-2024-002.pdf'),
(2, 'CERT-2024-003', '2024-02-25 11:00:00', '/certificates/cert-2024-003.pdf'),
(4, 'CERT-2024-004', '2024-03-01 09:15:00', '/certificates/cert-2024-004.pdf'),
(5, 'CERT-2024-005', '2024-03-05 16:20:00', '/certificates/cert-2024-005.pdf');

-- 27. QnA (depends on Courses, Users)
INSERT INTO [dbo].[QnA] ([CourseId], [UserId], [QuestionTitle], [QuestionText], [IsAnswered], [IsFeatured], [CreatedAt], [UpdatedAt]) VALUES
(1, 3, 'How to debug JavaScript errors?', 'I am getting a syntax error in my JavaScript code. Can someone help me understand how to debug this?', 1, 0, '2024-01-18 10:30:00', '2024-01-18 10:30:00'),
(1, 4, 'CSS Grid vs Flexbox', 'When should I use CSS Grid instead of Flexbox for layout?', 1, 1, '2024-01-20 14:15:00', '2024-01-20 14:15:00'),
(2, 3, 'React Hooks best practices', 'What are the best practices for using React hooks in functional components?', 0, 0, '2024-01-22 09:45:00', '2024-01-22 09:45:00'),
(2, 4, 'State management in React', 'Should I use Context API or Redux for state management in a large React application?', 1, 0, '2024-01-25 16:20:00', '2024-01-25 16:20:00'),
(3, 5, 'Design system components', 'How do I create a consistent design system for my web application?', 0, 0, '2024-01-28 11:10:00', '2024-01-28 11:10:00');

-- 28. QnAAnswers (depends on QnA, Users)
INSERT INTO [dbo].[QnAAnswers] ([QnAId], [UserId], [AnswerText], [IsAccepted], [Votes], [CreatedAt], [UpdatedAt]) VALUES
(1, 2, 'Use the browser developer tools console to check for JavaScript errors. Look for red error messages and check the line numbers.', 1, 5, '2024-01-18 11:00:00', '2024-01-18 11:00:00'),
(2, 2, 'Use CSS Grid for two-dimensional layouts (rows and columns) and Flexbox for one-dimensional layouts (either row or column).', 1, 8, '2024-01-20 15:30:00', '2024-01-20 15:30:00'),
(3, 2, 'Always use hooks at the top level of your component, never inside loops or conditions. Use useEffect for side effects.', 0, 3, '2024-01-22 10:15:00', '2024-01-22 10:15:00'),
(4, 2, 'For large applications, Redux provides better state management with time-travel debugging. Context API is good for smaller apps.', 1, 6, '2024-01-25 17:45:00', '2024-01-25 17:45:00'),
(5, 2, 'Start with a design system like Material-UI or create your own component library with consistent colors, typography, and spacing.', 0, 2, '2024-01-28 12:30:00', '2024-01-28 12:30:00');

-- 29. Notifications (depends on Users)
INSERT INTO [dbo].[Notifications] ([UserId], [Title], [Message], [Type], [IsRead], [Url], [CreatedAt]) VALUES
(3, 'Welcome to the course!', 'You have successfully enrolled in Complete Web Development Bootcamp', 'Enrollment', 1, '/course/1', '2024-01-15 10:00:00'),
(3, 'New lesson available', 'Lesson 3: JavaScript Programming is now available', 'Course Update', 0, '/lesson/3', '2024-01-18 09:00:00'),
(4, 'Course completed!', 'Congratulations! You have completed the Complete Web Development Bootcamp', 'Achievement', 1, '/certificate/1', '2024-02-15 16:45:00'),
(4, 'Certificate ready', 'Your certificate is ready for download', 'Certificate', 1, '/certificate/download/1', '2024-02-15 17:00:00'),
(5, 'Payment reminder', 'Your payment for Digital Marketing Strategy course is pending', 'Payment', 0, '/payment/3', '2024-01-26 10:00:00');

-- 30. ActivityLogs (depends on Users)
INSERT INTO [dbo].[ActivityLogs] ([UserId], [Action], [EntityType], [EntityId], [Details], [IpAddress], [UserAgent], [CreatedAt]) VALUES
(3, 'Login', 'User', 3, 'User logged in successfully', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36', '2024-01-15 10:00:00'),
(3, 'Enroll', 'Course', 1, 'Enrolled in Complete Web Development Bootcamp', '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36', '2024-01-15 10:05:00'),
(4, 'Complete', 'Lesson', 1, 'Completed lesson: Welcome to Web Development', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36', '2024-01-16 10:30:00'),
(4, 'Review', 'Course', 1, 'Posted a review for Complete Web Development Bootcamp', '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36', '2024-01-25 10:00:00'),
(5, 'Purchase', 'Order', 3, 'Purchased Digital Marketing Strategy course', '192.168.1.102', 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36', '2024-01-25 11:20:00');

-- 31. PasswordResetTokens (depends on Users)
INSERT INTO [dbo].[PasswordResetTokens] ([UserId], [Token], [ExpiresAt], [IsUsed], [CreatedAt]) VALUES
(3, 'reset_token_123456789', '2024-01-16 10:00:00', 1, '2024-01-15 10:00:00'),
(4, 'reset_token_987654321', '2024-01-17 14:30:00', 0, '2024-01-16 14:30:00'),
(5, 'reset_token_456789123', '2024-01-18 09:15:00', 1, '2024-01-17 09:15:00'),
(3, 'reset_token_789123456', '2024-01-20 11:20:00', 0, '2024-01-19 11:20:00'),
(4, 'reset_token_321654987', '2024-01-22 16:45:00', 1, '2024-01-21 16:45:00');

-- 32. SystemAnnouncements (depends on Users)
INSERT INTO [dbo].[SystemAnnouncements] ([Title], [Content], [Type], [IsActive], [StartDate], [EndDate], [CreatedBy], [CreatedAt]) VALUES
('Welcome to Our Learning Platform!', 'We are excited to have you join our community of learners. Start your journey with our comprehensive courses.', 'General', 1, '2024-01-01 00:00:00', '2024-12-31 23:59:59', 1, '2024-01-01 00:00:00'),
('New Course: Advanced React Development', 'Check out our latest course on advanced React concepts including hooks, context, and performance optimization.', 'Course', 1, '2024-01-15 00:00:00', '2024-12-31 23:59:59', 1, '2024-01-15 00:00:00'),
('System Maintenance Scheduled', 'We will be performing system maintenance on Sunday, February 4th from 2 AM to 4 AM EST.', 'Maintenance', 1, '2024-01-30 00:00:00', '2024-02-04 23:59:59', 1, '2024-01-30 00:00:00'),
('Student Success Stories', 'Read about how our students have transformed their careers with our courses. Check out the success stories section.', 'Success', 1, '2024-02-01 00:00:00', '2024-12-31 23:59:59', 1, '2024-02-01 00:00:00'),
('Mobile App Update Available', 'Download the latest version of our mobile app for improved performance and new features.', 'App', 1, '2024-02-05 00:00:00', '2024-12-31 23:59:59', 1, '2024-02-05 00:00:00');

-- 33. AdminMenu (no dependencies)
INSERT INTO [dbo].[AdminMenu] ([ItemName], [ItemLevel], [ParentLevel], [ItemOrder], [IsActive], [ItemTarget], [AreaName], [ControllerName], [ActionName], [Icon], [IdName], [CreatedAt], [MenuType]) VALUES
('Dashboard', 1, 0, 1, 1, '_self', 'Admin', 'Dashboard', 'Index', '/uploads/categories/default.jpg', 'dashboard', '2024-01-01 00:00:00', 'Main'),
('Users', 1, 0, 2, 1, '_self', 'Admin', 'Users', 'Users', '/uploads/categories/default.jpg', 'users', '2024-01-01 00:00:00', 'Main'),
('Courses', 1, 0, 3, 1, '_self', 'Admin', 'Courses', 'Index', '/uploads/categories/default.jpg', 'courses', '2024-01-01 00:00:00', 'Main'),
('Orders', 1, 0, 4, 1, '_self', 'Admin', 'Orders', 'Index', '/uploads/categories/default.jpg', 'orders', '2024-01-01 00:00:00', 'Main'),
('Reports', 1, 0, 5, 1, '_self', 'Admin', 'Reports', 'Index', '/uploads/categories/default.jpg', 'reports', '2024-01-01 00:00:00', 'Main'),
('User Management', 2, 2, 1, 1, '_self', 'Admin', 'Users', 'Index', '/uploads/categories/default.jpg', 'user-mgmt', '2024-01-01 00:00:00', 'Sub'),
('Role Management', 2, 2, 2, 1, '_self', 'Admin', 'Roles', 'Index', '/uploads/categories/default.jpg', 'role-mgmt', '2024-01-01 00:00:00', 'Sub'),
('Course Management', 2, 3, 1, 1, '_self', 'Admin', 'Courses', 'Index', '/uploads/categories/default.jpg', 'course-mgmt', '2024-01-01 00:00:00', 'Sub'),
('Category Management', 2, 3, 2, 1, '_self', 'Admin', 'Categories', 'Index', '/uploads/categories/default.jpg', 'category-mgmt', '2024-01-01 00:00:00', 'Sub'),
('Order Management', 2, 4, 1, 1, '_self', 'Admin', 'Orders', 'Index', '/uploads/categories/default.jpg', 'order-mgmt', '2024-01-01 00:00:00', 'Sub');