using System.Linq;
using System.Threading.Tasks;
using EngineersNotebook.Data;
using EngineersNotebook.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineersNotebook.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly ApplicationDbContext _context;

        public TagController(ILogger<TagController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(await _context.Tags.ToArrayAsync());
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (await _context.Tags.AnyAsync(x => x.Name == name))
            {
                ModelState.AddModelError("Name", "Name already taken");
                return View();
            }

            var model = new Tag { Name = name };
            
            _context.Tags.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = model.Id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _context.Tags.FindAsync(id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (!ModelState.IsValid)
                return View(tag);

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Tags.FindAsync(id);

            if (result == null)
                return NotFound();

            _context.Tags.Remove(result);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}