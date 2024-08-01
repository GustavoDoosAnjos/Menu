using Microsoft.AspNetCore.Mvc;
using Menu.Data;
using Menu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Menu.Controllers
{
    public class Menu : Controller
    {
        private readonly MenuContext _context;
        public Menu(MenuContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var dishes = from d in _context.Dishes
                       select d;
            if(!string.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(d => d.Name.Contains(searchString));
            }
            return View(await dishes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var dish = await _context.Dishes
                .Include(di => di.DishIngredient)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        public async Task<IActionResult> Create()
        {
            var dishes = from d in _context.Dishes
                         select d;
            return View(await dishes.ToListAsync());
        }

        public async Task<IActionResult> Post(Dish dish)
        {
            if (dish == null)
            {
                return BadRequest();
            }
            await _context.Dishes.AddAsync(dish);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
