using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduFlex.Areas.Admin.ViewModels
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Mô tả")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Icon hiện tại")]
        public string? CurrentIcon { get; set; }

        [Display(Name = "Thay icon mới")]
        public IFormFile? IconFile { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}