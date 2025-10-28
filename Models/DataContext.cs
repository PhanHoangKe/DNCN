using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduFlex.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EduFlex.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<CourseLevel> CourseLevels { get; set; }

    public virtual DbSet<CourseObjective> CourseObjectives { get; set; }

    public virtual DbSet<CourseRequirement> CourseRequirements { get; set; }

    public virtual DbSet<CourseReview> CourseReviews { get; set; }

    public virtual DbSet<CourseView> CourseViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__Activity__5E54864801D3DF79");

                entity.Property(e => e.Action).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.EntityType).HasMaxLength(50);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.UserAgent).HasMaxLength(500);

                entity.HasOne(d => d.Users).WithMany(p => p.ActivityLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ActivityLogs_Users");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.AnswerId).HasName("PK__Answers__D4825004F05E9A1F");

                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.IsCorrect).HasDefaultValue(false);

                entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answers_Questions");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A5DE4350D");

                entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_CartItems_UserCourse").IsUnique();

                entity.Property(e => e.AddedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course).WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartItems_Courses");

                entity.HasOne(d => d.Users).WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartItems_Users");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BBF255A00");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Icon).HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_Categories_Parent");
            });

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.CertificateId).HasName("PK__Certific__BBF8A7C16E9F9D30");

                entity.HasIndex(e => e.CertificateCode, "UQ__Certific__9B855830F9140548").IsUnique();

                entity.Property(e => e.CertificateCode).HasMaxLength(50);
                entity.Property(e => e.IssuedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.PdfUrl).HasMaxLength(500);

                entity.HasOne(d => d.Enrollment).WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.EnrollmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Certificates_Enrollments");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.CouponId).HasName("PK__Coupons__384AF1BA9B7250A4");

                entity.HasIndex(e => e.CouponCode, "UQ__Coupons__D3490800DE35FC2E").IsUnique();

                entity.Property(e => e.CouponCode).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.DiscountType).HasMaxLength(20);
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.MaxDiscount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.MinOrderAmount)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.UsageLimit).HasDefaultValue(0);
                entity.Property(e => e.UsedCount).HasDefaultValue(0);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A767B6F617");

                entity.HasIndex(e => e.Slug, "UQ__Courses__BC7B5FB600C09084").IsUnique();

                entity.Property(e => e.AverageRating)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(3, 2)");
                entity.Property(e => e.CourseTitle).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.DiscountPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Duration).HasDefaultValue(0);
                entity.Property(e => e.EnrollmentCount).HasDefaultValue(0);
                entity.Property(e => e.IsApproved).HasDefaultValue(false);
                entity.Property(e => e.IsFree).HasDefaultValue(true);
                entity.Property(e => e.IsPublished).HasDefaultValue(false);
                entity.Property(e => e.Language)
                    .HasMaxLength(50)
                    .HasDefaultValue("Tiếng Việt");
                entity.Property(e => e.PreviewVideoUrl).HasMaxLength(500);
                entity.Property(e => e.Price)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ShortDescription).HasMaxLength(500);
                entity.Property(e => e.Slug).HasMaxLength(255);
                entity.Property(e => e.ThumbnailUrl).HasMaxLength(500);
                entity.Property(e => e.TotalLessons).HasDefaultValue(0);
                entity.Property(e => e.TotalRatings).HasDefaultValue(0);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ViewCount).HasDefaultValue(0);

                entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.CourseApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_Courses_ApprovedBy");

                entity.HasOne(d => d.Categories).WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Courses_Categories");

                entity.HasOne(d => d.Instructor).WithMany(p => p.CourseInstructors)
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Courses_Instructors");

                entity.HasOne(d => d.Level).WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Courses_Levels");
            });

            modelBuilder.Entity<CourseLevel>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__CourseLe__09F03C264C6C92CC");

                entity.HasIndex(e => e.LevelName, "UQ__CourseLe__9EF3BE7BBADC7EF0").IsUnique();

                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.LevelName).HasMaxLength(50);
            });

            modelBuilder.Entity<CourseObjective>(entity =>
            {
                entity.HasKey(e => e.ObjectiveId).HasName("PK__CourseOb__8C5633AD999E80AB");

                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.Objective).HasMaxLength(500);

                entity.HasOne(d => d.Course).WithMany(p => p.CourseObjectives)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseObjectives_Courses");
            });

            modelBuilder.Entity<CourseRequirement>(entity =>
            {
                entity.HasKey(e => e.RequirementId).HasName("PK__CourseRe__7DF11E5DE4163B0D");

                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.Requirement).HasMaxLength(500);

                entity.HasOne(d => d.Course).WithMany(p => p.CourseRequirements)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseRequirements_Courses");
            });

            modelBuilder.Entity<CourseReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId).HasName("PK__CourseRe__74BC79CEEED9387F");

                entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_CourseReviews_UserCourse").IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsApproved).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course).WithMany(p => p.CourseReviews)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseReviews_Courses");

                entity.HasOne(d => d.Users).WithMany(p => p.CourseReviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseReviews_Users");
            });

            modelBuilder.Entity<CourseView>(entity =>
            {
                entity.HasKey(e => e.ViewId).HasName("PK__CourseVi__1E371CF60300E8D4");

                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.UserAgent).HasMaxLength(500);
                entity.Property(e => e.ViewedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course).WithMany(p => p.CourseViews)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseViews_Courses");

                entity.HasOne(d => d.Users).WithMany(p => p.CourseViews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CourseViews_Users");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F68771BFC32E57C");

                entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_Enrollments_UserCourse").IsUnique();

                entity.Property(e => e.EnrolledAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsCertificateIssued).HasDefaultValue(false);
                entity.Property(e => e.Progress)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Courses");

                entity.HasOne(d => d.Users).WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Users");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACD0BFD2C8E3");

                entity.Property(e => e.ContentUrl).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.Duration).HasDefaultValue(0);
                entity.Property(e => e.IsFree).HasDefaultValue(false);
                entity.Property(e => e.LessonTitle).HasMaxLength(255);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.VideoUrl).HasMaxLength(500);

                entity.HasOne(d => d.Section).WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_Lessons_Sections");

                entity.HasOne(d => d.Type).WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Types");
            });

            modelBuilder.Entity<LessonAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId).HasName("PK__LessonAt__442C64BE554BF81A");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.FileName).HasMaxLength(255);
                entity.Property(e => e.FileType).HasMaxLength(50);
                entity.Property(e => e.FileUrl).HasMaxLength(500);

                entity.HasOne(d => d.Lesson).WithMany(p => p.LessonAttachments)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_LessonAttachments_Lessons");
            });

            modelBuilder.Entity<LessonComment>(entity =>
            {
                entity.HasKey(e => e.CommentId).HasName("PK__LessonCo__C3B4DFCA666A3773");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsApproved).HasDefaultValue(true);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Lesson).WithMany(p => p.LessonComments)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_LessonComments_Lessons");

                entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                    .HasForeignKey(d => d.ParentCommentId)
                    .HasConstraintName("FK_LessonComments_Parent");

                entity.HasOne(d => d.Users).WithMany(p => p.LessonComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonComments_Users");
            });

            modelBuilder.Entity<LessonProgress>(entity =>
            {
                entity.HasKey(e => e.ProgressId).HasName("PK__LessonPr__BAE29CA5A4DC4072");

                entity.ToTable("LessonProgress");

                entity.HasIndex(e => new { e.EnrollmentId, e.LessonId }, "UQ_LessonProgress_EnrollmentLesson").IsUnique();

                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
                entity.Property(e => e.LastWatchedPosition).HasDefaultValue(0);

                entity.HasOne(d => d.Enrollment).WithMany(p => p.LessonProgresses)
                    .HasForeignKey(d => d.EnrollmentId)
                    .HasConstraintName("FK_LessonProgress_Enrollments");

                entity.HasOne(d => d.Lesson).WithMany(p => p.LessonProgresses)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonProgress_Lessons");
            });

            modelBuilder.Entity<LessonType>(entity =>
            {
                entity.HasKey(e => e.TypeId).HasName("PK__LessonTy__516F03B58AE5ED9E");

                entity.HasIndex(e => e.TypeName, "UQ__LessonTy__D4E7DFA822F38061").IsUnique();

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12F7A7EDDA");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Url).HasMaxLength(500);

                entity.HasOne(d => d.Users).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Users");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFB1CC4589");

                entity.HasIndex(e => e.OrderCode, "UQ__Orders__999B5229CCB0193F").IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.DiscountAmount)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FinalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OrderCode).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50)
                    .HasDefaultValue("Pending");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TransactionId).HasMaxLength(255);

                entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C14025B8C");

                entity.Property(e => e.DiscountPrice)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Course).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Courses");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasKey(e => e.TokenId).HasName("PK__Password__658FEEEA4681D1E9");

                entity.HasIndex(e => e.Token, "UQ__Password__1EB4F817B7134CA7").IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsUsed).HasDefaultValue(false);
                entity.Property(e => e.Token).HasMaxLength(255);

                entity.HasOne(d => d.Users).WithMany(p => p.PasswordResetTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PasswordResetTokens_Users");
            });

            modelBuilder.Entity<QnA>(entity =>
            {
                entity.HasKey(e => e.QnAid).HasName("PK__QnA__C4DF8B092E343571");

                entity.ToTable("QnA");

                entity.Property(e => e.QnAid).HasColumnName("QnAId");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsAnswered).HasDefaultValue(false);
                entity.Property(e => e.IsFeatured).HasDefaultValue(false);
                entity.Property(e => e.QuestionTitle).HasMaxLength(255);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course).WithMany(p => p.QnAs)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QnA_Courses");

                entity.HasOne(d => d.Users).WithMany(p => p.QnAs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QnA_Users");
            });

            modelBuilder.Entity<QnAanswer>(entity =>
            {
                entity.HasKey(e => e.AnswerId).HasName("PK__QnAAnswe__D482500404784D5C");

                entity.ToTable("QnAAnswers");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsAccepted).HasDefaultValue(false);
                entity.Property(e => e.QnAid).HasColumnName("QnAId");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Votes).HasDefaultValue(0);

                entity.HasOne(d => d.QnA).WithMany(p => p.QnAanswers)
                    .HasForeignKey(d => d.QnAid)
                    .HasConstraintName("FK_QnAAnswers_QnA");

                entity.HasOne(d => d.Users).WithMany(p => p.QnAanswers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QnAAnswers_Users");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC5174B7AA");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.Points).HasDefaultValue(1);
                entity.Property(e => e.QuestionType)
                    .HasMaxLength(50)
                    .HasDefaultValue("single");

                entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_Questions_Quizzes");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(e => e.QuizId).HasName("PK__Quizzes__8B42AE8E8D47A882");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.MaxAttempts).HasDefaultValue(0);
                entity.Property(e => e.PassingScore)
                    .HasDefaultValue(70m)
                    .HasColumnType("decimal(5, 2)");
                entity.Property(e => e.QuizTitle).HasMaxLength(255);
                entity.Property(e => e.ShowCorrectAnswers).HasDefaultValue(true);

                entity.HasOne(d => d.Lesson).WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_Quizzes_Lessons");
            });

            modelBuilder.Entity<QuizAttempt>(entity =>
            {
                entity.HasKey(e => e.AttemptId).HasName("PK__QuizAtte__891A68E609515C3C");

                entity.Property(e => e.CorrectAnswers).HasDefaultValue(0);
                entity.Property(e => e.IsPassed).HasDefaultValue(false);
                entity.Property(e => e.Score)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(5, 2)");
                entity.Property(e => e.StartedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.TotalQuestions).HasDefaultValue(0);

                entity.HasOne(d => d.Quiz).WithMany(p => p.QuizAttempts)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizAttempts_Quizzes");

                entity.HasOne(d => d.Users).WithMany(p => p.QuizAttempts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizAttempts_Users");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A0781A0E2");

                entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61602EB120A0").IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.SectionId).HasName("PK__Sections__80EF0872D9DE5BB7");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
                entity.Property(e => e.SectionTitle).HasMaxLength(255);

                entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Sections_Courses");
            });

            modelBuilder.Entity<StudentAnswer>(entity =>
            {
                entity.HasKey(e => e.StudentAnswerId).HasName("PK__StudentA__6E3EA40553611591");

                entity.Property(e => e.AnsweredAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsCorrect).HasDefaultValue(false);

                entity.HasOne(d => d.Answer).WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentAnswers_Answers");

                entity.HasOne(d => d.Attempt).WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.AttemptId)
                    .HasConstraintName("FK_StudentAnswers_Attempts");

                entity.HasOne(d => d.Question).WithMany(p => p.StudentAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentAnswers_Questions");
            });

            modelBuilder.Entity<SystemAnnouncement>(entity =>
            {
                entity.HasKey(e => e.AnnouncementId).HasName("PK__SystemAn__9DE44574BDC69BBC");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasDefaultValue("Info");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SystemAnnouncements)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemAnnouncements_CreatedBy");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CA48F585A");

                entity.HasIndex(e => e.Email, "UQ__Users__A9D105347E9AFCDD").IsUnique();

                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.Bio).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.EmailVerified).HasDefaultValue(false);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}