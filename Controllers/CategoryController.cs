using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel categoryModel = _dataContext.Categories.Where(p => p.Slug == Slug).FirstOrDefault();
			
			if (categoryModel == null) return RedirectToAction("Index");

			var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == categoryModel.Id);

			return View(await productsByCategory.OrderByDescending(p => p.Id).ToListAsync());
		}
    }
}
