using System;
using System.Linq;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonsController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly ILogger<LessonsController> _logger;
        
        public LessonsController(EduFlexContext context, ILogger<LessonsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lessons = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .Include(l => l.Type)
                .OrderByDescending(l => l.CreatedAt)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lesson lesson)
        {
            if (!ModelState.IsValid)
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
                
                ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName", lesson?.SectionId);
                
                ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName", lesson?.TypeId);
                return View(lesson);
            }

            lesson.CreatedAt = DateTime.Now.ToUniversalTime();
            lesson.UpdatedAt = DateTime.Now.ToUniversalTime();
            
            if (lesson.DisplayOrder == null)
            {
                var maxOrder = _context.Lessons
                    .Where(l => l.SectionId == lesson.SectionId)
                    .Max(l => (int?)l.DisplayOrder) ?? 0;
                lesson.DisplayOrder = maxOrder + 1;
            }

            _context.Lessons.Add(lesson);
            _context.SaveChanges();

            TempData["Success"] = "Th?m b?i gi?ng th?nh c?ng!";
            return RedirectToAction(nameof(Index));
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

            if (!ModelState.IsValid)
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
                
                ViewBag.Sections = new SelectList(sections, "SectionId", "SectionName", updatedLesson.SectionId);
                
                ViewBag.LessonTypes = new SelectList(_context.LessonTypes.OrderBy(t => t.TypeName), "TypeId", "TypeName", updatedLesson.TypeId);
                return View(updatedLesson);
            }

            // C?p nh?t c?c tr??ng
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
            TempData["Success"] = "C?p nh?t b?i gi?ng th?nh c?ng!";
            return RedirectToAction(nameof(Index));
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

            // Ki?m tra c?c quan h?
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
                return NotFound();
            }

            try
            {
                // X?a c?c d? li?u li?n quan
                if (lesson.LessonAttachments.Any())
                {
                    _context.LessonAttachments.RemoveRange(lesson.LessonAttachments);
                }
                
                if (lesson.LessonComments.Any())
                {
                    _context.LessonComments.RemoveRange(lesson.LessonComments);
                }
                
                if (lesson.LessonProgresses.Any())
                {
                    _context.LessonProgresses.RemoveRange(lesson.LessonProgresses);
                }

                // X?a quizzes v? d? li?u li?n quan c?a quiz
                if (lesson.Quizzes.Any())
                {
                    foreach (var quiz in lesson.Quizzes)
                    {
                        var questions = _context.Questions.Where(q => q.QuizId == quiz.QuizId).ToList();
                        foreach (var question in questions)
                        {
                            var answers = _context.Answers.Where(a => a.QuestionId == question.QuestionId).ToList();
                            _context.Answers.RemoveRange(answers);
                        }
                        _context.Questions.RemoveRange(questions);
                        
                        var attempts = _context.QuizAttempts.Where(qa => qa.QuizId == quiz.QuizId).ToList();
                        foreach (var attempt in attempts)
                        {
                            var studentAnswers = _context.StudentAnswers.Where(sa => sa.AttemptId == attempt.AttemptId).ToList();
                            _context.StudentAnswers.RemoveRange(studentAnswers);
                        }
                        _context.QuizAttempts.RemoveRange(attempts);
                    }
                    _context.Quizzes.RemoveRange(lesson.Quizzes);
                }

                // X?a b?i gi?ng
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();

                TempData["Success"] = $"?? x?a b?i gi?ng '{lesson.LessonTitle}' th?nh c?ng!";
                _logger.LogInformation("Lesson {LessonId} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson {LessonId}", id);
                TempData["Error"] = $"L?i khi x?a b?i gi?ng: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
