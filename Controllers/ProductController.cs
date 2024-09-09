using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int Id) 
        {      
            if (Id == null) 
                return RedirectToAction("Index");
            else
            {
				var productById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();


                var rproduct = await _dataContext.Products.Where(p => p.BrandId == productById.BrandId && p.Id != productById.Id).Take(3).ToListAsync();

                ViewBag.rProducts = rproduct;
				return View(productById);
			}				
		}
    }
}
