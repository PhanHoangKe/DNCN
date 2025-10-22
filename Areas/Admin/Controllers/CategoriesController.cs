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

            category.CreatedAt = DateTime.Now.ToUniversalTime();
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

            // Lấy danh sách khóa học liên quan
            var relatedCourses = _context.Courses
                .Where(c => c.CategoryId == id)
                .Select(c => new { c.CourseId, c.CourseTitle, c.IsPublished })
                .ToList();

            ViewBag.RelatedCourses = relatedCourses;
            ViewBag.RelatedCoursesCount = relatedCourses.Count;
            ViewBag.HasChildCategories = _context.Categories.Any(c => c.ParentCategoryId == id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, bool? deleteRelatedCourses = false)
        {
            Console.WriteLine($"DeleteConfirmed called with id: {id}, deleteRelatedCourses: {deleteRelatedCourses}");
            
            var category = _context.Categories.Find(id);
            if (category == null) 
            {
                Console.WriteLine("Category not found");
                return NotFound();
            }

            // Kiểm tra xem có danh mục con không
            var hasChildCategories = _context.Categories.Any(c => c.ParentCategoryId == id);
            Console.WriteLine($"Has child categories: {hasChildCategories}");
            if (hasChildCategories)
            {
                Console.WriteLine("Cannot delete - has child categories");
                TempData["Error"] = "Không thể xóa danh mục này vì còn có danh mục con!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem có khóa học nào thuộc danh mục này không
            var relatedCourses = _context.Courses.Where(c => c.CategoryId == id).ToList();
            Console.WriteLine($"Related courses count: {relatedCourses.Count}, deleteRelatedCourses: {deleteRelatedCourses}");
            if (relatedCourses.Any() && deleteRelatedCourses != true)
            {
                Console.WriteLine("Cannot delete - has related courses but not confirmed");
                TempData["Error"] = "Không thể xóa danh mục này vì còn có khóa học thuộc danh mục này!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có khóa học liên quan và người dùng xác nhận xóa
            if (relatedCourses.Any() && deleteRelatedCourses == true)
            {
                Console.WriteLine("Deleting related courses and all related data...");
                
                try
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Sử dụng raw SQL để xóa tất cả dữ liệu liên quan
                            var courseIds = string.Join(",", relatedCourses.Select(c => c.CourseId));
                            
                            Console.WriteLine($"Deleting all related data for courses: {courseIds}");
                            
                            // Xóa CartItems
                            _context.Database.ExecuteSqlRaw($"DELETE FROM CartItems WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted cart items for courses: {courseIds}");
                            
                            // Xóa OrderDetails
                            _context.Database.ExecuteSqlRaw($"DELETE FROM OrderDetails WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted order details for courses: {courseIds}");
                            
                            // Xóa Enrollments
                            _context.Database.ExecuteSqlRaw($"DELETE FROM Enrollments WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted enrollments for courses: {courseIds}");
                            
                            // Xóa CourseReviews
                            _context.Database.ExecuteSqlRaw($"DELETE FROM CourseReviews WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted course reviews for courses: {courseIds}");
                            
                            // Xóa CourseViews
                            _context.Database.ExecuteSqlRaw($"DELETE FROM CourseViews WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted course views for courses: {courseIds}");
                            
                            // Xóa QnAs
                            _context.Database.ExecuteSqlRaw($"DELETE FROM QnAs WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted QnAs for courses: {courseIds}");
                            
                            // Xóa CourseObjectives
                            _context.Database.ExecuteSqlRaw($"DELETE FROM CourseObjectives WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted course objectives for courses: {courseIds}");
                            
                            // Xóa CourseRequirements
                            _context.Database.ExecuteSqlRaw($"DELETE FROM CourseRequirements WHERE CourseId IN ({courseIds})");
                            Console.WriteLine($"Deleted course requirements for courses: {courseIds}");
                            
                    // Xóa Sections và tất cả dữ liệu con của nó
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE sa FROM StudentAnswers sa 
                        INNER JOIN Lessons l ON sa.LessonId = l.LessonId 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted StudentAnswers");
                    
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE qa FROM QuizAttempts qa 
                        INNER JOIN Lessons l ON qa.LessonId = l.LessonId 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted QuizAttempts");
                    
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE lc FROM LessonComments lc 
                        INNER JOIN Lessons l ON lc.LessonId = l.LessonId 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted LessonComments");
                    
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE lp FROM LessonProgresses lp 
                        INNER JOIN Lessons l ON lp.LessonId = l.LessonId 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted LessonProgresses");
                    
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE la FROM LessonAttachments la 
                        INNER JOIN Lessons l ON la.LessonId = l.LessonId 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted LessonAttachments");
                    
                    _context.Database.ExecuteSqlRaw($@"
                        DELETE l FROM Lessons l 
                        INNER JOIN Sections s ON l.SectionId = s.SectionId 
                        WHERE s.CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted Lessons");
                    
                    _context.Database.ExecuteSqlRaw($"DELETE FROM Sections WHERE CourseId IN ({courseIds})");
                    Console.WriteLine("Deleted Sections");
                            
                            // Cuối cùng xóa các khóa học
                            _context.Courses.RemoveRange(relatedCourses);
                            Console.WriteLine($"Deleted {relatedCourses.Count} courses");
                            
                            // Commit transaction
                            transaction.Commit();
                            Console.WriteLine("Transaction committed successfully");
                            
                            TempData["Success"] = $"Đã xóa danh mục '{category.CategoryName}' và {relatedCourses.Count} khóa học liên quan cùng tất cả dữ liệu liên quan!";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error in transaction, rolling back: {ex.Message}");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting related data: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    TempData["Error"] = $"Lỗi khi xóa dữ liệu liên quan: {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                Console.WriteLine("No related courses to delete");
                TempData["Success"] = $"Đã xóa danh mục '{category.CategoryName}' thành công!";
            }

            // Xóa danh mục
            Console.WriteLine("Deleting category...");
            _context.Categories.Remove(category);
            _context.SaveChanges();
            Console.WriteLine("Category deleted successfully");

            return RedirectToAction(nameof(Index));
        }
    }
}