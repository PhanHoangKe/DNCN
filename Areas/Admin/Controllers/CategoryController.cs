using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduFlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var category = _context.Categories.OrderBy(c => c.CategoryId).ToList();
            return View(category);
        }
    }
}