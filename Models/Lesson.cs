using System.ComponentModel.DataAnnotations;

namespace EduFlex.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int SectionId { get; set; }

    [Required(ErrorMessage = "Tiêu đề bài giảng không được để trống")]
    [StringLength(255, ErrorMessage = "Tiêu đề bài giảng không được vượt quá 255 ký tự")]
    [RegularExpression(@"^[^@#$%^&*+=\\/<>?!]*$",ErrorMessage = "Tiêu đề không được chứa các ký tự đặc biệt như @#$%^&*+=\\/<>?!")]
    public string LessonTitle { get; set; } = null!; public string? Description { get; set; }

    public int TypeId { get; set; }

    public string? ContentUrl { get; set; }

    public string? VideoUrl { get; set; }

    public int? Duration { get; set; }

    public bool? IsFree { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<LessonAttachment> LessonAttachments { get; set; } = new List<LessonAttachment>();

    public virtual ICollection<LessonComment> LessonComments { get; set; } = new List<LessonComment>();

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual Section Section { get; set; } = null!;

    public virtual LessonType Type { get; set; } = null!;
}
