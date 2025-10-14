using System;
using System.Linq;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly EduFlexContext _context;
        public UsersController(EduFlexContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User users, IFormFile? avatarFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(_context.Roles, "RoleId", "RoleName", users?.RoleId);
                return View(users);
            }

            users.CreatedAt = DateTime.Now;
            users.UpdatedAt = DateTime.Now;
            users.PasswordHash = BCrypt.Net.BCrypt.HashPassword(users.PasswordHash);
            users.EmailVerified = false;
            users.LastLoginAt = null;

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avatars");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                avatarFile.CopyTo(stream);
                users.Avatar = "/uploads/avatars/" + fileName;
            }
            else
            {
                users.Avatar = "/uploads/avatars/default.jpg";
            }

            _context.Users.Add(users);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            ViewBag.Roles = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User updatedUser, IFormFile? avatarFile)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            // Nếu mật khẩu để trống, tạm gán mật khẩu cũ vào ModelState để qua validate
            if (string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
                updatedUser.PasswordHash = user.PasswordHash;

            // Kiểm tra ModelState sau khi gán
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(_context.Roles, "RoleId", "RoleName", updatedUser.RoleId);
                return View(updatedUser);
            }

            // Cập nhật các trường
            user.FullName = updatedUser.FullName;
            user.Bio = updatedUser.Bio;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.RoleId = updatedUser.RoleId;
            user.IsActive = updatedUser.IsActive;
            user.UpdatedAt = DateTime.Now;

            // Nếu nhập mật khẩu mới, hash lại
            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash) && updatedUser.PasswordHash != user.PasswordHash)
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);

            // Upload avatar mới nếu có
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avatars");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                avatarFile.CopyTo(stream);
                user.Avatar = "/uploads/avatars/" + fileName;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
