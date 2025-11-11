using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EduFlex.Models;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuizzesController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly ILogger<QuizzesController> _logger;

        public QuizzesController(EduFlexContext context, ILogger<QuizzesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var quizzes = _context.Quizzes
                .Include(q => q.Lesson)
                    .ThenInclude(l => l.Section)
                        .ThenInclude(s => s.Course)
                .OrderByDescending(q => q.CreatedAt ?? DateTime.MinValue)
                .ToList();
            return View(quizzes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Prepare lessons list for selection
            var lessons = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .OrderBy(l => l.Section.Course.CourseTitle)
                .ThenBy(l => l.Section.SectionTitle)
                .ThenBy(l => l.LessonTitle)
                .Select(l => new { LessonId = l.LessonId, LessonName = $"{l.Section.Course.CourseTitle} - {l.Section.SectionTitle} - {l.LessonTitle}" })
                .ToList();

            ViewBag.Lessons = new SelectList(lessons, "LessonId", "LessonName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                quiz.CreatedAt = DateTime.Now.ToUniversalTime();
                _context.Quizzes.Add(quiz);
                _context.SaveChanges();
                TempData["Success"] = "Thêm bài kiểm tra thành công!";
                _logger.LogInformation("Quiz created: {QuizTitle}", quiz.QuizTitle);
                return RedirectToAction(nameof(Index));
            }

            // repopulate select lists
            var lessons = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .OrderBy(l => l.Section.Course.CourseTitle)
                .ThenBy(l => l.Section.SectionTitle)
                .ThenBy(l => l.LessonTitle)
                .Select(l => new { LessonId = l.LessonId, LessonName = $"{l.Section.Course.CourseTitle} - {l.Section.SectionTitle} - {l.LessonTitle}" })
                .ToList();

            ViewBag.Lessons = new SelectList(lessons, "LessonId", "LessonName", quiz.LessonId);
            return View(quiz);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var quiz = _context.Quizzes
                .Include(q => q.Lesson)
                .FirstOrDefault(q => q.QuizId == id);
            if (quiz == null) return NotFound();

            var lessons = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .OrderBy(l => l.Section.Course.CourseTitle)
                .ThenBy(l => l.Section.SectionTitle)
                .ThenBy(l => l.LessonTitle)
                .Select(l => new { LessonId = l.LessonId, LessonName = $"{l.Section.Course.CourseTitle} - {l.Section.SectionTitle} - {l.LessonTitle}" })
                .ToList();

            ViewBag.Lessons = new SelectList(lessons, "LessonId", "LessonName", quiz.LessonId);
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Quiz updatedQuiz)
        {
            var quiz = _context.Quizzes.Find(id);
            if (quiz == null) return NotFound();

            if (!ModelState.IsValid)
            {
                quiz.QuizTitle = updatedQuiz.QuizTitle;
                quiz.Description = updatedQuiz.Description;
                quiz.LessonId = updatedQuiz.LessonId;
                quiz.TimeLimit = updatedQuiz.TimeLimit;
                quiz.PassingScore = updatedQuiz.PassingScore;
                quiz.MaxAttempts = updatedQuiz.MaxAttempts;
                quiz.ShowCorrectAnswers = updatedQuiz.ShowCorrectAnswers;

                _context.SaveChanges();
                TempData["Success"] = "Cập nhật bài kiểm tra thành công!";
                _logger.LogInformation("Quiz {QuizId} updated", id);
                return RedirectToAction(nameof(Index));
            }

            var lessons = _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .OrderBy(l => l.Section.Course.CourseTitle)
                .ThenBy(l => l.Section.SectionTitle)
                .ThenBy(l => l.LessonTitle)
                .Select(l => new { LessonId = l.LessonId, LessonName = $"{l.Section.Course.CourseTitle} - {l.Section.SectionTitle} - {l.LessonTitle}" })
                .ToList();

            ViewBag.Lessons = new SelectList(lessons, "LessonId", "LessonName", updatedQuiz.LessonId);
            return View(updatedQuiz);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var quiz = _context.Quizzes
                .Include(q => q.Lesson)
                    .ThenInclude(l => l.Section)
                        .ThenInclude(s => s.Course)
                .Include(q => q.Questions)
                .Include(q => q.QuizAttempts)
                .FirstOrDefault(q => q.QuizId == id);
            if (quiz == null) return NotFound();

            ViewBag.RelatedCount = quiz.Questions.Count + quiz.QuizAttempts.Count;
            return View(quiz);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var quiz = _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.QuizAttempts)
                .FirstOrDefault(q => q.QuizId == id);
            if (quiz == null) return NotFound();

            try
            {
                // Remove related entities
                if (quiz.QuizAttempts.Any())
                {
                    var attempts = _context.QuizAttempts.Where(a => a.QuizId == id).ToList();
                    foreach (var at in attempts)
                    {
                        var studentAnswers = _context.StudentAnswers.Where(sa => sa.AttemptId == at.AttemptId).ToList();
                        _context.StudentAnswers.RemoveRange(studentAnswers);
                    }
                    _context.QuizAttempts.RemoveRange(attempts);
                }

                if (quiz.Questions.Any())
                {
                    var questionIds = quiz.Questions.Select(q => q.QuestionId).ToList();
                    var answers = _context.Answers.Where(a => questionIds.Contains(a.QuestionId)).ToList();
                    _context.Answers.RemoveRange(answers);
                    _context.Questions.RemoveRange(quiz.Questions);
                }

                _context.Quizzes.Remove(quiz);
                _context.SaveChanges();
                TempData["Success"] = "Xóa bài kiểm tra thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz {QuizId}", id);
                TempData["Error"] = $"Lỗi khi xóa bài kiểm tra: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
