using System;
using System.IO;
using System.Linq;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly EduFlexContext _context;

        public CourseController(EduFlexContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Level)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName");
            ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName");
            ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course, IFormFile? courseFile)
        {
            ModelState.Remove("Price");
            ModelState.Remove("DiscountPrice");
            ModelState.Remove("Duration");
            ModelState.Remove("TotalLessons");
            ModelState.Remove("Language");
            ModelState.Remove("ShortDescription");
            ModelState.Remove("FullDescription");
            ModelState.Remove("PreviewVideoUrl");
            ModelState.Remove("ThumbnailUrl");
            ModelState.Remove("Category");
            ModelState.Remove("Instructor");
            ModelState.Remove("Level");
            ModelState.Remove("ApprovedByNavigation");
            
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", course.CategoryId);
                ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName", course.InstructorId);
                ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName", course.LevelId);
                return View(course);
            }

            if (_context.Courses.Any(c => c.Slug == course.Slug))
            {
                ModelState.AddModelError("Slug", "Slug này đã được sử dụng. Vui lòng chọn slug khác.");
                ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", course.CategoryId);
                ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName", course.InstructorId);
                ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName", course.LevelId);
                return View(course);
            }

            course.CreatedAt = DateTime.Now;
            course.UpdatedAt = DateTime.Now;
            course.IsPublished = false;
            course.IsApproved = false;
            course.ViewCount = 0;
            course.EnrollmentCount = 0;
            course.AverageRating = 0;
            course.TotalRatings = 0;

            if (courseFile != null && courseFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(courseFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/course");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                courseFile.CopyTo(stream);
                course.ThumbnailUrl = "/uploads/course/" + fileName;
            }
            else
            {
                course.ThumbnailUrl = "/uploads/course/default.png";
            }

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", course.CategoryId);
            ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName", course.InstructorId);
            ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName", course.LevelId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course updatedCourse, IFormFile? courseFile)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            ModelState.Remove("Price");
            ModelState.Remove("DiscountPrice");
            ModelState.Remove("Duration");
            ModelState.Remove("TotalLessons");
            ModelState.Remove("Language");
            ModelState.Remove("ShortDescription");
            ModelState.Remove("FullDescription");
            ModelState.Remove("PreviewVideoUrl");
            ModelState.Remove("ThumbnailUrl");
            ModelState.Remove("Category");
            ModelState.Remove("Instructor");
            ModelState.Remove("Level");
            ModelState.Remove("ApprovedByNavigation");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", updatedCourse.CategoryId);
                ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName", updatedCourse.InstructorId);
                ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName", updatedCourse.LevelId);
                return View(updatedCourse);
            }

            if (_context.Courses.Any(c => c.Slug == updatedCourse.Slug && c.CourseId != id))
            {
                ModelState.AddModelError("Slug", "Slug này đã được sử dụng. Vui lòng chọn slug khác.");
                ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsActive == true), "CategoryId", "CategoryName", updatedCourse.CategoryId);
                ViewBag.Instructors = new SelectList(_context.Users.Where(u => u.IsActive == true), "UserId", "FullName", updatedCourse.InstructorId);
                ViewBag.Levels = new SelectList(_context.CourseLevels, "LevelId", "LevelName", updatedCourse.LevelId);
                return View(updatedCourse);
            }

            course.CourseTitle = updatedCourse.CourseTitle;
            course.Slug = updatedCourse.Slug;
            course.ShortDescription = updatedCourse.ShortDescription;
            course.FullDescription = updatedCourse.FullDescription;
            course.Price = updatedCourse.Price;
            course.DiscountPrice = updatedCourse.DiscountPrice;
            course.IsFree = updatedCourse.IsFree;
            course.CategoryId = updatedCourse.CategoryId;
            course.InstructorId = updatedCourse.InstructorId;
            course.LevelId = updatedCourse.LevelId;
            course.Language = updatedCourse.Language;
            course.Duration = updatedCourse.Duration;
            course.TotalLessons = updatedCourse.TotalLessons;
            course.IsPublished = updatedCourse.IsPublished;
            course.UpdatedAt = DateTime.Now;

            if (courseFile != null && courseFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(courseFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/course");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                courseFile.CopyTo(stream);
                course.ThumbnailUrl = "/uploads/course/" + fileName;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Level)
                .FirstOrDefault(c => c.CourseId == id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}