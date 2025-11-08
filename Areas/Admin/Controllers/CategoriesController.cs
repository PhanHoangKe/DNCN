using EduFlex.Models;
using EduFlex.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(EduFlexContext context, IWebHostEnvironment env, ILogger<CategoriesController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.ParentCategory)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentCategories = new SelectList(
                _context.Categories.Where(c => c.IsActive == true),
                "CategoryId", "CategoryName");
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(
                    _context.Categories.Where(c => c.IsActive == true),
                    "CategoryId", "CategoryName", model.ParentCategoryId);
                return View(model);
            }

            var category = new Category
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
                ParentCategoryId = model.ParentCategoryId,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            category.Icon = await SaveIconAsync(model.IconFile);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new CategoryEditViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                CurrentIcon = category.Icon,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive ?? true
            };

            ViewBag.ParentCategories = new SelectList(
                _context.Categories.Where(c => c.IsActive == true && c.CategoryId != id),
                "CategoryId", "CategoryName", category.ParentCategoryId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditViewModel model)
        {
            if (id != model.CategoryId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ParentCategories = new SelectList(
                    _context.Categories.Where(c => c.IsActive == true && c.CategoryId != id),
                    "CategoryId", "CategoryName", model.ParentCategoryId);
                return View(model);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            category.CategoryName = model.CategoryName;
            category.Description = model.Description;
            category.ParentCategoryId = model.ParentCategoryId;
            category.IsActive = model.IsActive;

            if (model.IconFile != null && model.IconFile.Length > 0)
            {
                // Xóa icon cũ
                if (!string.IsNullOrEmpty(category.Icon) && 
                    System.IO.File.Exists(Path.Combine(_env.WebRootPath, category.Icon.TrimStart('/'))))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, category.Icon.TrimStart('/')));
                }

                category.Icon = await SaveIconAsync(model.IconFile);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            var relatedCoursesCount = await _context.Courses.CountAsync(c => c.CategoryId == id);
            var hasChild = await _context.Categories.AnyAsync(c => c.ParentCategoryId == id);

            ViewBag.RelatedCoursesCount = relatedCoursesCount;
            ViewBag.HasChildCategories = hasChild;
            ViewBag.CategoryName = category.CategoryName;

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, bool deleteRelatedCourses = false)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var hasChild = await _context.Categories.AnyAsync(c => c.ParentCategoryId == id);
            if (hasChild)
            {
                TempData["Error"] = "Không thể xóa! Danh mục này đang có danh mục con.";
                return RedirectToAction(nameof(Index));
            }

            var relatedCourses = await _context.Courses.Where(c => c.CategoryId == id).ToListAsync();

            if (relatedCourses.Any() && !deleteRelatedCourses)
            {
                TempData["Error"] = $"Danh mục có {relatedCourses.Count} khóa học. Tick xác nhận để xóa tất cả!";
                return RedirectToAction(nameof(Index));
            }

            if (relatedCourses.Any())
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var courseIds = relatedCourses.Select(c => c.CourseId).ToList();
                    var idsParam = string.Join(",", courseIds);

                    // XÓA TẤT CẢ DỮ LIỆU LIÊN QUAN – DÙNG RAW SQL NHANH + AN TOÀN
                    var sqlCommands = new[]
                    {
                        "DELETE FROM StudentAnswers WHERE AttemptId IN (SELECT AttemptId FROM QuizAttempts WHERE QuizId IN (SELECT QuizId FROM Quizzes WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0}))))",
                        "DELETE FROM QuizAttempts WHERE QuizId IN (SELECT QuizId FROM Quizzes WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0}))))",
                        "DELETE FROM LessonComments WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0})))",
                        "DELETE FROM LessonProgresses WHERE EnrollmentId IN (SELECT EnrollmentId FROM Enrollments WHERE CourseId IN ({0}))",
                        "DELETE FROM LessonAttachments WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0})))",
                        "DELETE FROM Answers WHERE QuestionId IN (SELECT QuestionId FROM Questions WHERE QuizId IN (SELECT QuizId FROM Quizzes WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0}))))",
                        "DELETE FROM Questions WHERE QuizId IN (SELECT QuizId FROM Quizzes WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0}))))",
                        "DELETE FROM Quizzes WHERE LessonId IN (SELECT LessonId FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0})))",
                        "DELETE FROM Lessons WHERE SectionId IN (SELECT SectionId FROM Sections WHERE CourseId IN ({0}))",
                        "DELETE FROM Sections WHERE CourseId IN ({0})",
                        "DELETE FROM CartItems WHERE CourseId IN ({0})",
                        "DELETE FROM OrderDetails WHERE CourseId IN ({0})",
                        "DELETE FROM Enrollments WHERE CourseId IN ({0})",
                        "DELETE FROM CourseReviews WHERE CourseId IN ({0})",
                        "DELETE FROM CourseViews WHERE CourseId IN ({0})",
                        "DELETE FROM QnAAnswers WHERE QnAId IN (SELECT QnAId FROM QnAs WHERE CourseId IN ({0}))",
                        "DELETE FROM QnAs WHERE CourseId IN ({0})",
                        "DELETE FROM CourseObjectives WHERE CourseId IN ({0})",
                        "DELETE FROM CourseRequirements WHERE CourseId IN ({0})",
                        "DELETE FROM Courses WHERE CourseId IN ({0})"
                    };

                    foreach (var sql in sqlCommands)
                    {
                        await _context.Database.ExecuteSqlRawAsync(string.Format(sql, idsParam));
                    }

                    await transaction.CommitAsync();
                    TempData["Success"] = $"Đã xóa danh mục và {relatedCourses.Count} khóa học + toàn bộ dữ liệu liên quan!";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Lỗi xóa dữ liệu liên quan category {Id}", id);
                    TempData["Error"] = "Lỗi hệ thống khi xóa dữ liệu!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Xóa icon nếu có
            if (!string.IsNullOrEmpty(category.Icon))
            {
                var filePath = Path.Combine(_env.WebRootPath, category.Icon.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = TempData["Success"] ?? $"Đã xóa danh mục '{category.CategoryName}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Helper: lưu icon
        private async Task<string?> SaveIconAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "categories");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/categories/{fileName}";
        }
    }
}