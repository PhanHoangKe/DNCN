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
    public class CategoriesController : Controller
    {
        private readonly EduFlexContext _context;
        public CategoriesController(EduFlexContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories
                .Include(c => c.ParentCategory)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, IFormFile? iconFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", category?.ParentCategoryId);
                return View(category);
            }

            category.CreatedAt = DateTime.Now;
            category.IsActive = true;

            if (iconFile != null && iconFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(iconFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/categories");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                iconFile.CopyTo(stream);
                category.Icon = "/uploads/categories/" + fileName;
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => c.IsActive == true && c.CategoryId != id), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category updatedCategory, IFormFile? iconFile)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => c.IsActive == true && c.CategoryId != id), "CategoryId", "CategoryName", updatedCategory.ParentCategoryId);
                return View(updatedCategory);
            }

            // Cập nhật các trường
            category.CategoryName = updatedCategory.CategoryName;
            category.Description = updatedCategory.Description;
            category.ParentCategoryId = updatedCategory.ParentCategoryId;
            category.IsActive = updatedCategory.IsActive;

            // Upload icon mới nếu có
            if (iconFile != null && iconFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(iconFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/categories");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                iconFile.CopyTo(stream);
                category.Icon = "/uploads/categories/" + fileName;
            }

            _context.SaveChanges();
            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            // Kiểm tra xem có danh mục con không
            var hasChildCategories = _context.Categories.Any(c => c.ParentCategoryId == id);
            if (hasChildCategories)
            {
                TempData["Error"] = "Không thể xóa danh mục này vì còn có danh mục con!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem có khóa học nào thuộc danh mục này không
            var hasCourses = _context.Courses.Any(c => c.CategoryId == id);
            if (hasCourses)
            {
                TempData["Error"] = "Không thể xóa danh mục này vì còn có khóa học thuộc danh mục này!";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            TempData["Success"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}