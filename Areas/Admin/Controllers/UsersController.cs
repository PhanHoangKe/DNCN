using EduFlex.Models;
using EduFlex.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller  
    {
        private readonly EduFlexContext _context;
        private readonly IWebHostEnvironment _env;

        public UsersController(EduFlexContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_context.Roles.AsNoTracking(), "RoleId", "RoleName");
            return View(new UserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = new SelectList(_context.Roles.AsNoTracking(), "RoleId", "RoleName", model.RoleId);
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                ViewData["Roles"] = new SelectList(_context.Roles.AsNoTracking(), "RoleId", "RoleName", model.RoleId);
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Bio = model.Bio,
                RoleId = model.RoleId ?? 0,
                IsActive = model.IsActive,
                EmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            user.Avatar = await SaveAvatarAsync(model.AvatarFile) ?? "/uploads/avatars/default.jpg";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tạo tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var viewModel = new UserEditViewModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                CurrentAvatar = user.Avatar,
                RoleId = user.RoleId,
                IsActive = user.IsActive ?? true
            };

            ViewData["Roles"] = new SelectList(_context.Roles.AsNoTracking(), "RoleId", "RoleName", user.RoleId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel model)
        {
            if (id != model.UserId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = new SelectList(_context.Roles.AsNoTracking(), "RoleId", "RoleName", model.RoleId);
                return View(model);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.UserId != id))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng bởi tài khoản khác.");
                ViewData["Roles"] = new SelectList(_context.Roles, "RoleId", "RoleName", model.RoleId);
                return View(model);
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Bio = model.Bio;
            user.RoleId = model.RoleId;
            user.IsActive = model.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            }

            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.Avatar) && 
                    user.Avatar.Contains("/uploads/avatars/") && 
                    !user.Avatar.Contains("default"))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, user.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                user.Avatar = await SaveAvatarAsync(model.AvatarFile);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Avatar) && 
                    user.Avatar.Contains("/uploads/avatars/") && 
                    !user.Avatar.Contains("default"))
                {
                    var filePath = Path.Combine(_env.WebRootPath, user.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Xóa tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string?> SaveAvatarAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/avatars/{fileName}";
        }
    }
}