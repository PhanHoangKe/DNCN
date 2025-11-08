using EduFlex.Areas.Admin.ViewModels;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly EduFlexContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(EduFlexContext context, IWebHostEnvironment env, ILogger<CoursesController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Level)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new CourseCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model, IFormFile? courseFile)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View(model);
            }

            if (await _context.Courses.AnyAsync(c => c.Slug == model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug này đã tồn tại!");
                await PopulateDropdowns(model);
                return View(model);
            }

            var course = new Course
            {
                CourseTitle = model.CourseTitle,
                Slug = model.Slug,
                ShortDescription = model.ShortDescription,
                FullDescription = model.FullDescription,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                IsFree = model.IsFree,
                CategoryId = model.CategoryId,
                InstructorId = model.InstructorId,
                LevelId = model.LevelId,
                Language = model.Language,
                Duration = model.Duration,
                TotalLessons = model.TotalLessons,
                IsPublished = model.IsPublished,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsApproved = false,
                ViewCount = 0,
                EnrollmentCount = 0,
                AverageRating = 0,
                TotalRatings = 0
            };

            course.ThumbnailUrl = await SaveThumbnailAsync(courseFile);

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã tạo khóa học \"{course.CourseTitle}\" thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var vm = new CourseEditViewModel
            {
                CourseId = course.CourseId,
                CourseTitle = course.CourseTitle,
                Slug = course.Slug,
                ShortDescription = course.ShortDescription,
                FullDescription = course.FullDescription,
                Price = course.Price,
                DiscountPrice = course.DiscountPrice,
                IsFree = course.IsFree ?? false,
                CategoryId = course.CategoryId,
                InstructorId = course.InstructorId,
                LevelId = course.LevelId,
                Language = course.Language,
                Duration = course.Duration,
                TotalLessons = course.TotalLessons,
                IsPublished = course.IsPublished ?? false,
                CurrentThumbnail = course.ThumbnailUrl
            };

            await PopulateDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseEditViewModel model, IFormFile? courseFile)
        {
            if (id != model.CourseId) return NotFound();

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model);
                return View(model);
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (await _context.Courses.AnyAsync(c => c.Slug == model.Slug && c.CourseId != id))
            {
                ModelState.AddModelError("Slug", "Slug này đã được dùng!");
                await PopulateDropdowns(model);
                return View(model);
            }

            // Cập nhật
            course.CourseTitle = model.CourseTitle;
            course.Slug = model.Slug;
            course.ShortDescription = model.ShortDescription;
            course.FullDescription = model.FullDescription;
            course.Price = model.Price;
            course.DiscountPrice = model.DiscountPrice;
            course.IsFree = model.IsFree;
            course.CategoryId = model.CategoryId;
            course.InstructorId = model.InstructorId;
            course.LevelId = model.LevelId;
            course.Language = model.Language;
            course.Duration = model.Duration;
            course.TotalLessons = model.TotalLessons;
            course.IsPublished = model.IsPublished;
            course.UpdatedAt = DateTime.UtcNow;

            if (courseFile != null && courseFile.Length > 0)
            {
                // Xóa ảnh cũ
                if (!string.IsNullOrEmpty(course.ThumbnailUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, course.ThumbnailUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                course.ThumbnailUrl = await SaveThumbnailAsync(courseFile);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string?> SaveThumbnailAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return "/uploads/course/default.png";

            var folder = Path.Combine(_env.WebRootPath, "uploads", "course");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/course/{fileName}";
        }

        private async Task PopulateDropdowns(object? model = null)
        {
            ViewBag.Category = new SelectList(
                await _context.Categories.Where(c => c.IsActive == true).ToListAsync(),
                "CategoryId", "CategoryName",
                model is CourseCreateViewModel c ? c.CategoryId : (model as CourseEditViewModel)?.CategoryId);

            ViewBag.Instructors = new SelectList(
                await _context.Users.Where(u => u.IsActive == true).ToListAsync(),
                "UserId", "FullName",
                model is CourseCreateViewModel ci ? ci.InstructorId : (model as CourseEditViewModel)?.InstructorId);

            ViewBag.Levels = new SelectList(
                await _context.CourseLevels.ToListAsync(),
                "LevelId", "LevelName",
                model is CourseCreateViewModel cl ? cl.LevelId : (model as CourseEditViewModel)?.LevelId);
        }
    }
}