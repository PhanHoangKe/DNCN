using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EduFlex.Models;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonsController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly ILogger<LessonsController> _logger;

        public LessonsController(EduFlexContext context)
        {
            _context = context;
            _logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            }).CreateLogger<LessonsController>();
        }

        public IActionResult Index()
        {
            var lessons = _context.Lessons
                .Include(l => l.Section)
                .Include(s => s.Section.Course)
                .Include(l => l.Type)
                .OrderByDescending(l => l.CreatedAt ?? DateTime.MinValue)
                .ToList();
            return View(lessons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var sections = _context.Sections
                .Include(s => s.Course)
                .OrderBy(s => s.Course.CourseTitle)
                .ThenBy(s => s.DisplayOrder)
                .Select(s => new { 
                    SectionId = s.SectionId, 
                    SectionName = $"{s.Course.CourseTitle} - {s.SectionTitle}" 
                })
                .ToList();
            
            ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName");
            ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName");
            return View();
        }

        // POST: Admin/Lessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lesson lesson)
        {
            if (!ModelState.IsValid)
            {
                lesson.CreatedAt = DateTime.Now;
                _context.Lessons.Add(lesson);
                _context.SaveChanges();

                TempData["Success"] = "✅ Thêm bài giảng thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "❌ Không thể thêm bài giảng. Vui lòng kiểm tra lại thông tin.";
            ViewBag.Sections = new SelectList(_context.Sections, "SectionId", "SectionTitle", lesson.SectionId);
            ViewBag.LessonTypes = new SelectList(_context.LessonTypes, "TypeId", "TypeName", lesson.TypeId);
            return View(lesson);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lesson = _context.Lessons
                .Include(l => l.Section)
                .Include(l => l.Type)
                .FirstOrDefault(l => l.LessonId == id);

            if (lesson == null) return NotFound();
            var sections = _context.Sections
                .Include(s => s.Course)
                .OrderBy(s => s.Course.CourseTitle)
                .ThenBy(s => s.DisplayOrder)
                .Select(s => new { 
                    SectionId = s.SectionId, 
                    SectionName = $"{s.Course.CourseTitle} - {s.SectionTitle}" 
                })
                .ToList();
            
            ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName", lesson.SectionId);
            ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName", lesson.TypeId);
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lesson updatedLesson)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null) return NotFound();

            try
            {
                // Xử lý IsFree từ form
                var isFreeValues = Request.Form["IsFree"];
                updatedLesson.IsFree = isFreeValues.Any(v => v == "true");

                if (!ModelState.IsValid)
                {
                    // Cập nhật các trường
                    lesson.LessonTitle = updatedLesson.LessonTitle;
                    lesson.Description = updatedLesson.Description;
                    lesson.SectionId = updatedLesson.SectionId;
                    lesson.TypeId = updatedLesson.TypeId;
                    lesson.ContentUrl = updatedLesson.ContentUrl;
                    lesson.VideoUrl = updatedLesson.VideoUrl;
                    lesson.Duration = updatedLesson.Duration;
                    lesson.IsFree = updatedLesson.IsFree;
                    lesson.DisplayOrder = updatedLesson.DisplayOrder;
                    lesson.UpdatedAt = DateTime.Now.ToUniversalTime();

                    _context.SaveChanges();
                    TempData["Success"] = "Cập nhật bài giảng thành công!";
                    return RedirectToAction(nameof(Index));
                }

                var sections = _context.Sections
                    .Include(s => s.Course)
                    .OrderBy(s => s.Course.CourseTitle)
                    .ThenBy(s => s.DisplayOrder)
                    .Select(s => new { 
                        SectionId = s.SectionId, 
                        SectionName = $"{s.Course.CourseTitle} - {s.SectionTitle}" 
                    })
                    .ToList();
                
                ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName", updatedLesson.SectionId);
                ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName", updatedLesson.TypeId);
                return View(updatedLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson {LessonId}", id);
                TempData["Error"] = $"Lỗi khi cập nhật bài giảng: {ex.Message}";
                
                var sections = _context.Sections
                    .Include(s => s.Course)
                    .OrderBy(s => s.Course.CourseTitle)
                    .ThenBy(s => s.DisplayOrder)
                    .Select(s => new { 
                        SectionId = s.SectionId, 
                        SectionName = $"{s.Course.CourseTitle} - {s.SectionTitle}" 
                    })
                    .ToList();
                
                ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName", updatedLesson.SectionId);
                ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName", updatedLesson.TypeId);
                
                return View(updatedLesson);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lesson = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .Include(l => l.Type)
                .Include(l => l.LessonAttachments)
                .Include(l => l.LessonComments)
                .Include(l => l.LessonProgresses)
                .Include(l => l.Quizzes)
                .FirstOrDefault(l => l.LessonId == id);
            
            if (lesson == null) return NotFound();

            // Kiểm tra các quan hệ
            ViewBag.HasAttachments = lesson.LessonAttachments.Any();
            ViewBag.HasComments = lesson.LessonComments.Any();
            ViewBag.HasProgress = lesson.LessonProgresses.Any();
            ViewBag.HasQuizzes = lesson.Quizzes.Any();
            ViewBag.RelatedCount = lesson.LessonAttachments.Count + 
                                   lesson.LessonComments.Count + 
                                   lesson.LessonProgresses.Count + 
                                   lesson.Quizzes.Count;

            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var lesson = _context.Lessons
                .Include(l => l.LessonAttachments)
                .Include(l => l.LessonComments)
                .Include(l => l.LessonProgresses)
                .Include(l => l.Quizzes)
                .FirstOrDefault(l => l.LessonId == id);
            
            if (lesson == null) 
            {
                _logger.LogWarning("Lesson with id {LessonId} not found", id);
                TempData["Error"] = "Bài giảng không tồn tại hoặc đã bị xóa!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa StudentAnswers liên quan đến Quizzes của Lesson này
                        var quizIds = lesson.Quizzes.Select(q => q.QuizId).ToList();
                        if (quizIds.Any())
                        {
                            var attempts = _context.QuizAttempts
                                .Where(qa => quizIds.Contains(qa.QuizId))
                                .Select(qa => qa.AttemptId)
                                .ToList();

                            if (attempts.Any())
                            {
                                var studentAnswers = _context.StudentAnswers
                                    .Where(sa => attempts.Contains(sa.AttemptId))
                                    .ToList();
                                _context.StudentAnswers.RemoveRange(studentAnswers);
                                _logger.LogInformation("Deleted {Count} StudentAnswers", studentAnswers.Count);
                            }

                            // 2. Xóa QuizAttempts
                            var quizAttempts = _context.QuizAttempts
                                .Where(qa => quizIds.Contains(qa.QuizId))
                                .ToList();
                            _context.QuizAttempts.RemoveRange(quizAttempts);
                            _logger.LogInformation("Deleted {Count} QuizAttempts", quizAttempts.Count);

                            // 3. Xóa Answers và Questions của Quizzes
                            var questions = _context.Questions
                                .Where(q => quizIds.Contains(q.QuizId))
                                .ToList();

                            var questionIds = questions.Select(q => q.QuestionId).ToList();
                            if (questionIds.Any())
                            {
                                var answers = _context.Answers
                                    .Where(a => questionIds.Contains(a.QuestionId))
                                    .ToList();
                                _context.Answers.RemoveRange(answers);
                                _logger.LogInformation("Deleted {Count} Answers", answers.Count);
                            }

                            _context.Questions.RemoveRange(questions);
                            _logger.LogInformation("Deleted {Count} Questions", questions.Count);

                            // 4. Xóa Quizzes
                            _context.Quizzes.RemoveRange(lesson.Quizzes);
                            _logger.LogInformation("Deleted {Count} Quizzes", lesson.Quizzes.Count);
                        }

                        // 5. Xóa LessonComments
                        if (lesson.LessonComments.Any())
                        {
                            _context.LessonComments.RemoveRange(lesson.LessonComments);
                            _logger.LogInformation("Deleted {Count} LessonComments", lesson.LessonComments.Count);
                        }

                        // 6. Xóa LessonAttachments
                        if (lesson.LessonAttachments.Any())
                        {
                            _context.LessonAttachments.RemoveRange(lesson.LessonAttachments);
                            _logger.LogInformation("Deleted {Count} LessonAttachments", lesson.LessonAttachments.Count);
                        }

                        // 7. Xóa LessonProgresses (phải xóa trước khi xóa Lesson vì có FK)
                        if (lesson.LessonProgresses.Any())
                        {
                            _context.LessonProgresses.RemoveRange(lesson.LessonProgresses);
                            _logger.LogInformation("Deleted {Count} LessonProgresses", lesson.LessonProgresses.Count);
                        }

                        // 8. Cuối cùng xóa Lesson
                        _context.Lessons.Remove(lesson);
                        _logger.LogInformation("Deleting Lesson {LessonId}", id);

                        _context.SaveChanges();
                        transaction.Commit();

                        TempData["Success"] = $"Đã xóa bài giảng '{lesson.LessonTitle}' thành công!";
                        _logger.LogInformation("Lesson {LessonId} deleted successfully", id);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error in transaction, rolling back for lesson {LessonId}", id);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson {LessonId}", id);
                TempData["Error"] = $"Lỗi khi xóa bài giảng: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
