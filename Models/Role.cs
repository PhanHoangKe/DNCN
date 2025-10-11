using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduFlex.Models;

namespace EduFlex.Areas.Admin.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required, StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<User>? Users { get; set; }
    }
}
