using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EngineersNotebook.Data;
using EngineersNotebook.Data.Models;
using EngineersNotebook.Models.Documentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor.Templating.Core;

namespace EngineersNotebook.Controllers
{
    //[Route("{controller}")]
    public class DocumentationController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public DocumentationController(ApplicationDbContext context)
        {
            _context = context;
            
            // These are required for rendering the PDF / Razor pages
            RazorTemplateEngine.Initialize();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [HttpGet][HttpPost]
        public async Task<FileResult> FilterByTagPdf([FromBody]DocumentationTagFilter filter)
        {
            List<Documentation> docs = await _context.Docs
                .Include(x => x.CreatedByUser)
                .Include(x => x.EditedByUser)
                .Include(x => x.Tags)
                .ToListAsync();

            Console.WriteLine(filter);
            
            if (filter is not null)
            {
                if (filter.TagIds is not null)
                    docs = docs.Where(x => x.Tags
                            .Select(y => y.Id)
                            .Intersect(filter.TagIds)
                            .Any())
                        .ToList();
                
                if (filter.TagNames is not null)
                {
                    // lowercase everything
                    filter.TagNames = filter.TagNames
                        .Select(x => x.ToLower())
                        .ToArray();
                    
                    docs = docs.Where(x => x.Tags
                            .Select(x => x.Name.ToLower())
                            .Intersect(filter.TagNames)
                            .Any())
                        .ToList();
                }
            }

            List<string> pages = new();
            
            // Render each individual documentation page
            foreach (var doc in docs)
                pages.Add(await Utility.ToViewString("Doc", doc));
            
            // Stitch each page together with multiple lines inbetween
            string compiled = string.Join("<br/><br/><br/>", pages);
            
            // Convert to PDF
            var result = await Utility.HtmlToPdf(compiled);
            return File(result, "application/pdf");
        }

        [HttpGet][HttpPost]
        public async Task<FileResult> ToPdf(int id)
        {
            var doc = await _context.Docs
                .Include(x => x.EditedByUser)
                .Include(x => x.CreatedByUser)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync();
            
            if(doc is null)
                return null;

            var result = await Utility.HtmlToPdf(await Utility.ToViewString("Doc", doc));
            return File(result, "application/pdf");
        }

        [HttpGet]
        public async Task<string> FilterByTag([FromBody] DocumentationTagFilter filter)
        {
            List<Documentation> docs = await _context.Docs
                                                            .Include(x => x.EditedByUser)
                                                            .Include(x => x.CreatedByUser)
                                                            .Include(x=>x.Tags)
                                                            .ToListAsync();

            if (filter is not null)
            {
                if (filter.TagIds is not null)
                    docs = docs.Where(x => x.Tags
                        .Select(y => y.Id)
                            .Intersect(filter.TagIds)
                            .Any())
                        .ToList();
                
                if (filter.TagNames is not null)
                {
                    // lowercase everything
                    filter.TagNames = filter.TagNames
                        .Select(x => x.ToLower())
                        .ToArray();
                    
                    docs = docs.Where(x => x.Tags
                        .Select(x => x.Name.ToLower())
                            .Intersect(filter.TagNames)
                            .Any())
                        .ToList();
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(docs.ToArray());
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var results = await _context.Docs
                .Include(x => x.CreatedByUser)
                .Include(x => x.EditedByUser)
                .Include(x=>x.Tags)
                .ToListAsync();
            
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _context.Docs
                .Include(x => x.CreatedByUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return NotFound();
            
            return View(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateDocumentation model = new();
            model.Tags = await _context.Tags.ToArrayAsync();
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            List<int> ids = new();
            if (collection.ContainsKey("tags"))
            {
                ids.AddRange(collection["tags"].Select(x=>int.Parse(x)));
            }

            string userId = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                .Value.ToString();
            
            var entity = new Documentation()
            {
                Description = collection["Description"],
                Title = collection["Title"],
                Contents = collection["Contents"],
                CreatedByUserId = userId,
                EditedByUserId = userId
            };

            entity.Tags.AddRange(await _context.Tags.Where(x=>ids.Contains(x.Id)).ToListAsync());

            _context.Docs.Add(entity);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Details), new { id = entity.Id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Docs.FindAsync(id);

            if (result == null)
                return NotFound();

            _context.Docs.Remove(result);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}