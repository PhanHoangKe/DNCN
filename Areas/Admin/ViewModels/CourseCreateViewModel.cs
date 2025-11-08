using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduFlex.Areas.Admin.ViewModels
{
    public class CourseCreateViewModel
{
    [Required] public string CourseTitle { get; set; } = null!;
    [Required] public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public decimal? Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public bool IsFree { get; set; }
    [Required] public int CategoryId { get; set; }
    [Required] public int InstructorId { get; set; }
    [Required] public int LevelId { get; set; }
    public string? Language { get; set; }
    public int? Duration { get; set; }
    public int? TotalLessons { get; set; }
    public bool IsPublished { get; set; }
}
}