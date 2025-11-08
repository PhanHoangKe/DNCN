using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EduFlex.Areas.Admin.Models;

namespace EduFlex.Models;

public partial class EduFlexContext : DbContext
{
    public EduFlexContext()
    {
    }

    public EduFlexContext(DbContextOptions<EduFlexContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<AdminMenu> AdminMenus { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseLevel> CourseLevels { get; set; }

    public virtual DbSet<CourseObjective> CourseObjectives { get; set; }

    public virtual DbSet<CourseRequirement> CourseRequirements { get; set; }

    public virtual DbSet<CourseReview> CourseReviews { get; set; }

    public virtual DbSet<CourseView> CourseViews { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonAttachment> LessonAttachments { get; set; }

    public virtual DbSet<LessonComment> LessonComments { get; set; }

    public virtual DbSet<LessonProgress> LessonProgresses { get; set; }

    public virtual DbSet<LessonType> LessonTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<QnA> QnAs { get; set; }

    public virtual DbSet<QnAanswer> QnAanswers { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<SystemAnnouncement> SystemAnnouncements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\HOANGKE;Initial Catalog=EduFlex;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__5E54864827D2B170");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ActivityLogs_Users");
        });

        // modelBuilder.Entity<AdminMenu>(entity =>
        // {
        //     entity.HasKey(e => e.AdminMenuId).HasName("PK__AdminMen__5DF61764AA56D0CC");

        //     entity.ToTable("AdminMenu");

        //     entity.Property(e => e.AdminMenuId).HasColumnName("AdminMenuID");
        //     entity.Property(e => e.ActionName).HasMaxLength(20);
        //     entity.Property(e => e.AreaName).HasMaxLength(20);
        //     entity.Property(e => e.ControllerName).HasMaxLength(20);
        //     entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        //     entity.Property(e => e.Icon).HasMaxLength(50);
        //     entity.Property(e => e.IdName).HasMaxLength(50);
        //     entity.Property(e => e.IsActive).HasDefaultValue(true);
        //     entity.Property(e => e.ItemName).HasMaxLength(50);
        //     entity.Property(e => e.ItemTarget).HasMaxLength(20);
        //     entity.Property(e => e.MenuType).HasMaxLength(20);
        // });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Answers__D4825004BC35C279");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.IsCorrect).HasDefaultValue(false);

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answers_Questions");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0AA5D8EC6A");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_CartItems_UserCourse").IsUnique();

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Courses");

            entity.HasOne(d => d.User).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B132B82A0");

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
            entity.HasKey(e => e.CertificateId).HasName("PK__Certific__BBF8A7C1313AEF58");

            entity.HasIndex(e => e.CertificateCode, "UQ__Certific__9B855830AC63C5C3").IsUnique();

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
            entity.HasKey(e => e.CouponId).HasName("PK__Coupons__384AF1BA1BF93FB0");

            entity.HasIndex(e => e.CouponCode, "UQ__Coupons__D349080058A770A2").IsUnique();

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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7F119A263");

            entity.HasIndex(e => e.CategoryId, "IX_Courses_CategoryId");

            entity.HasIndex(e => e.InstructorId, "IX_Courses_InstructorId");

            entity.HasIndex(e => e.IsPublished, "IX_Courses_IsPublished");

            entity.HasIndex(e => e.Price, "IX_Courses_Price");

            entity.HasIndex(e => e.Slug, "IX_Courses_Slug");

            entity.HasIndex(e => e.Slug, "UQ__Courses__BC7B5FB6570EA3CE").IsUnique();

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

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
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
            entity.HasKey(e => e.LevelId).HasName("PK__CourseLe__09F03C262D5A8C7C");

            entity.HasIndex(e => e.LevelName, "UQ__CourseLe__9EF3BE7B269967E1").IsUnique();

            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.LevelName).HasMaxLength(50);
        });

        modelBuilder.Entity<CourseObjective>(entity =>
        {
            entity.HasKey(e => e.ObjectiveId).HasName("PK__CourseOb__8C5633AD4EDAC9D2");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Objective).HasMaxLength(500);

            entity.HasOne(d => d.Course).WithMany(p => p.CourseObjectives)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseObjectives_Courses");
        });

        modelBuilder.Entity<CourseRequirement>(entity =>
        {
            entity.HasKey(e => e.RequirementId).HasName("PK__CourseRe__7DF11E5DC8475F5A");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Requirement).HasMaxLength(500);

            entity.HasOne(d => d.Course).WithMany(p => p.CourseRequirements)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseRequirements_Courses");
        });

        modelBuilder.Entity<CourseReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__CourseRe__74BC79CEE7C98E36");

            entity.HasIndex(e => e.CourseId, "IX_CourseReviews_CourseId");

            entity.HasIndex(e => e.Rating, "IX_CourseReviews_Rating");

            entity.HasIndex(e => e.UserId, "IX_CourseReviews_UserId");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "UQ_CourseReviews_UserCourse").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseReviews)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseReviews_Courses");

            entity.HasOne(d => d.User).WithMany(p => p.CourseReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseReviews_Users");
        });

        modelBuilder.Entity<CourseView>(entity =>
        {
            entity.HasKey(e => e.ViewId).HasName("PK__CourseVi__1E371CF6D9C1F77F");

            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.ViewedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseViews)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseViews_Courses");

            entity.HasOne(d => d.User).WithMany(p => p.CourseViews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CourseViews_Users");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F68771B59EF0F8E");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("tr_Enrollments_DecrementCount");
                    tb.HasTrigger("tr_Enrollments_IncrementCount");
                });

            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");

            entity.HasIndex(e => e.EnrolledAt, "IX_Enrollments_EnrolledAt");

            entity.HasIndex(e => e.UserId, "IX_Enrollments_UserId");

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

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Users");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACD0AA985DA9");

            entity.HasIndex(e => e.DisplayOrder, "IX_Lessons_DisplayOrder");

            entity.HasIndex(e => e.SectionId, "IX_Lessons_SectionId");

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
            entity.HasKey(e => e.AttachmentId).HasName("PK__LessonAt__442C64BE528D4C8C");

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
            entity.HasKey(e => e.CommentId).HasName("PK__LessonCo__C3B4DFCA25A85632");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonComments)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonComments_Lessons");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK_LessonComments_Parent");

            entity.HasOne(d => d.User).WithMany(p => p.LessonComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonComments_Users");
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__LessonPr__BAE29CA586552F8D");

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
            entity.HasKey(e => e.TypeId).HasName("PK__LessonTy__516F03B5D4DF56B5");

            entity.HasIndex(e => e.TypeName, "UQ__LessonTy__D4E7DFA838EF194E").IsUnique();

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12CBB3741A");

            entity.HasIndex(e => e.IsRead, "IX_Notifications_IsRead");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF5DD42151");

            entity.HasIndex(e => e.CreatedAt, "IX_Orders_CreatedAt");

            entity.HasIndex(e => e.PaymentStatus, "IX_Orders_PaymentStatus");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.HasIndex(e => e.OrderCode, "UQ__Orders__999B5229C79A56EA").IsUnique();

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

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C69BF2C3F");

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
            entity.HasKey(e => e.TokenId).HasName("PK__Password__658FEEEAC059C4F4");

            entity.HasIndex(e => e.Token, "UQ__Password__1EB4F817C92C9246").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);
            entity.Property(e => e.Token).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PasswordResetTokens_Users");
        });

        modelBuilder.Entity<QnA>(entity =>
        {
            entity.HasKey(e => e.QnAid).HasName("PK__QnA__C4DF8B095C03AFF2");

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

            entity.HasOne(d => d.User).WithMany(p => p.QnAs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QnA_Users");
        });

        modelBuilder.Entity<QnAanswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__QnAAnswe__D48250047B81E632");

            entity.ToTable("QnAAnswers");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAccepted).HasDefaultValue(false);
            entity.Property(e => e.QnAid).HasColumnName("QnAId");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Votes).HasDefaultValue(0);

            entity.HasOne(d => d.QnA).WithMany(p => p.QnAanswers)
                .HasForeignKey(d => d.QnAid)
                .HasConstraintName("FK_QnAAnswers_QnA");

            entity.HasOne(d => d.User).WithMany(p => p.QnAanswers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QnAAnswers_Users");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC85E770C9");

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
            entity.HasKey(e => e.QuizId).HasName("PK__Quizzes__8B42AE8EBF01E22D");

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
            entity.HasKey(e => e.AttemptId).HasName("PK__QuizAtte__891A68E6B8898BD2");

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

            entity.HasOne(d => d.User).WithMany(p => p.QuizAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuizAttempts_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ACF79B137");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160A1D5973A").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Sections__80EF0872798239AE");

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
            entity.HasKey(e => e.StudentAnswerId).HasName("PK__StudentA__6E3EA40533EA9227");

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
            entity.HasKey(e => e.AnnouncementId).HasName("PK__SystemAn__9DE4457454CD9147");

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C87E22E2A");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.IsActive, "IX_Users_IsActive");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534204EF534").IsUnique();

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
