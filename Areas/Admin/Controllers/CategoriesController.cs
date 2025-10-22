using System;
using System.Linq;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly ILogger<CategoriesController> _logger;
        
        public CategoriesController(EduFlexContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogInformation("DeleteConfirmed called with id: {CategoryId}, deleteRelatedCourses: {DeleteRelatedCourses}", id, deleteRelatedCourses);
            
            var category = _context.Categories.Find(id);
            if (category == null) 
            {
                _logger.LogWarning("Category with id {CategoryId} not found", id);
                return NotFound();
            }

            // Kiểm tra xem có danh mục con không
            var hasChildCategories = _context.Categories.Any(c => c.ParentCategoryId == id);
            _logger.LogInformation("Has child categories: {HasChildCategories}", hasChildCategories);
            if (hasChildCategories)
            {
                _logger.LogWarning("Cannot delete category {CategoryId} - has child categories", id);
                TempData["Error"] = "Không thể xóa danh mục này vì còn có danh mục con!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem có khóa học nào thuộc danh mục này không
            var relatedCourses = _context.Courses.Where(c => c.CategoryId == id).ToList();
            _logger.LogInformation("Related courses count: {RelatedCoursesCount}, deleteRelatedCourses: {DeleteRelatedCourses}", relatedCourses.Count, deleteRelatedCourses);
            if (relatedCourses.Any() && deleteRelatedCourses != true)
            {
                _logger.LogWarning("Cannot delete category {CategoryId} - has related courses but not confirmed", id);
                TempData["Error"] = "Không thể xóa danh mục này vì còn có khóa học thuộc danh mục này!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có khóa học liên quan và người dùng xác nhận xóa
            if (relatedCourses.Any() && deleteRelatedCourses == true)
            {
                _logger.LogInformation("Deleting related courses and all related data for category {CategoryId}", id);
                
                try
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var courseIds = relatedCourses.Select(c => c.CourseId).ToList();
                            
                            _logger.LogInformation("Deleting all related data for courses: {CourseIds}", string.Join(",", courseIds));
                            
                            // Xóa dữ liệu liên quan theo thứ tự đúng (từ bảng con đến bảng cha)
                            // 1. Xóa StudentAnswers (cần LessonId từ Lessons)
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE sa FROM StudentAnswers sa 
                                INNER JOIN QuizAttempts qa ON sa.AttemptId = qa.AttemptId
                                INNER JOIN Quizzes q ON qa.QuizId = q.QuizId
                                INNER JOIN Lessons l ON q.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted StudentAnswers");
                            
                            // 2. Xóa QuizAttempts (cần QuizId từ Quizzes)
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE qa FROM QuizAttempts qa 
                                INNER JOIN Quizzes q ON qa.QuizId = q.QuizId
                                INNER JOIN Lessons l ON q.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted QuizAttempts");
                            
                            // 3. Xóa LessonComments
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE lc FROM LessonComments lc 
                                INNER JOIN Lessons l ON lc.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted LessonComments");
                            
                            // 4. Xóa LessonProgresses (cần EnrollmentId, không phải LessonId)
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE lp FROM LessonProgresses lp 
                                INNER JOIN Enrollments e ON lp.EnrollmentId = e.EnrollmentId
                                WHERE e.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted LessonProgresses");
                            
                            // 5. Xóa LessonAttachments
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE la FROM LessonAttachments la 
                                INNER JOIN Lessons l ON la.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted LessonAttachments");
                            
                            // 6. Xóa Questions và Answers
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE a FROM Answers a
                                INNER JOIN Questions q ON a.QuestionId = q.QuestionId
                                INNER JOIN Quizzes qu ON q.QuizId = qu.QuizId
                                INNER JOIN Lessons l ON qu.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Answers");
                            
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE q FROM Questions q
                                INNER JOIN Quizzes qu ON q.QuizId = qu.QuizId
                                INNER JOIN Lessons l ON qu.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Questions");
                            
                            // 7. Xóa Quizzes
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE qu FROM Quizzes qu
                                INNER JOIN Lessons l ON qu.LessonId = l.LessonId 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Quizzes");
                            
                            // 8. Xóa Lessons
                            _context.Database.ExecuteSqlRaw(@"
                                DELETE l FROM Lessons l 
                                INNER JOIN Sections s ON l.SectionId = s.SectionId 
                                WHERE s.CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Lessons");
                            
                            // 9. Xóa Sections
                            _context.Database.ExecuteSqlRaw("DELETE FROM Sections WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Sections");
                            
                            // 10. Xóa các bảng liên quan trực tiếp đến Course
                            _context.Database.ExecuteSqlRaw("DELETE FROM CartItems WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted CartItems");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM OrderDetails WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted OrderDetails");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM Enrollments WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted Enrollments");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM CourseReviews WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted CourseReviews");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM CourseViews WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted CourseViews");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM QnAs WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted QnAs");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM QnAAnswers WHERE QnAId IN (SELECT QnAId FROM QnAs WHERE CourseId IN ({0}))", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted QnAAnswers");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM CourseObjectives WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted CourseObjectives");
                            
                            _context.Database.ExecuteSqlRaw("DELETE FROM CourseRequirements WHERE CourseId IN ({0})", string.Join(",", courseIds));
                            _logger.LogInformation("Deleted CourseRequirements");
                            
                            // 11. Cuối cùng xóa các khóa học
                            _context.Courses.RemoveRange(relatedCourses);
                            _logger.LogInformation("Deleted {CourseCount} courses", relatedCourses.Count);
                            
                            // Commit transaction
                            transaction.Commit();
                            _logger.LogInformation("Transaction committed successfully");
                            
                            TempData["Success"] = $"Đã xóa danh mục '{category.CategoryName}' và {relatedCourses.Count} khóa học liên quan cùng tất cả dữ liệu liên quan!";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError(ex, "Error in transaction, rolling back for category {CategoryId}", id);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting related data for category {CategoryId}", id);
                    TempData["Error"] = $"Lỗi khi xóa dữ liệu liên quan: {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                _logger.LogInformation("No related courses to delete for category {CategoryId}", id);
                TempData["Success"] = $"Đã xóa danh mục '{category.CategoryName}' thành công!";
            }

            // Xóa danh mục
            _logger.LogInformation("Deleting category {CategoryId}", id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            _logger.LogInformation("Category {CategoryId} deleted successfully", id);

            return RedirectToAction(nameof(Index));
        }
    }
}