using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CafeMenuProject.Models;
using Microsoft.AspNetCore.Authorization;
using CafeMenuProject.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeMenuProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => !c.IsDeleted), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedDate = DateTime.Now;
                category.CreatorUserId = (await _userManager.GetUserAsync(User))?.Id;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => !c.IsDeleted), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => !c.IsDeleted && c.CategoryId != id), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,ParentCategoryId")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.UpdatedDate = DateTime.Now;
                    category.UpdaterUserId = (await _userManager.GetUserAsync(User))?.Id;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentCategories = new SelectList(_context.Categories.Where(c => !c.IsDeleted && c.CategoryId != id), "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        // Diğer CRUD işlemleri için benzer metotlar ekleyin
    }
}
