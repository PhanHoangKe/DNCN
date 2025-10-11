using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduFlex.Areas.Admin.Models;

namespace EduFlex.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool EmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public virtual Role Role { get; set; }
    }
}