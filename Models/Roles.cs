using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduFlex.Models;

namespace EduFlex.Areas.Admin.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
