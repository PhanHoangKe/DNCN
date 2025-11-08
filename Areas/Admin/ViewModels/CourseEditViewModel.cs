using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduFlex.Areas.Admin.ViewModels
{
    public class CourseEditViewModel : CourseCreateViewModel
{
    public int CourseId { get; set; }
    public string? CurrentThumbnail { get; set; }
}
}