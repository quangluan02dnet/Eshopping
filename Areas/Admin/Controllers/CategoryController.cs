using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories.OrderBy(p => p.Id).ToListAsync());
        }
		public async Task<IActionResult> Edit(int id)
		{
            CategoryModel categoryModel = await _dataContext.Categories.FindAsync(id);

			return View(categoryModel);
		}

		public async Task<IActionResult> Add()
		{
			return View();
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(CategoryModel category)
		{
			if (ModelState.IsValid)
			{
				category.Slug = category.Name.Replace(" ", "_");
				var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Category has exites in DB");
					return View(category);
				}

				_dataContext.Categories.Add(category);

				TempData["success"] = "Add category successfully";

				await _dataContext.SaveChangesAsync();

				return RedirectToAction("Index");


			}
			else
			{
				TempData["error"] = "error";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}
			return View(category);
		}

		[HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
		{        
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "_");
                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update category successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(category);
        }


        public async Task<IActionResult> Remove(int id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Remove category successfully";
            return RedirectToAction("Index");
        }
    }
}
