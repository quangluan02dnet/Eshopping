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
    public class ProductController : Controller
	{

		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = dataContext;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderBy(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}



		public IActionResult Add()
        {
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }


		[HttpPost]
		[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name" , product.BrandId);
			if (ModelState.IsValid)
			{
				product.Slug = product.Name.Replace(" ", "_");
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug != null) 
				{
					ModelState.AddModelError("", "Product has exites in DB");
					return View();
				}
				
				if(product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "image/shop");
					string imageName =  Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imageName);
					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					product.Image = imageName;
				}

				_dataContext.Products.Add(product);

                TempData["success"] = "Add product successfully";

                await _dataContext.SaveChangesAsync();

                return RedirectToAction("Index");
               

            }
			else
			{
				TempData["error"] = "error";
				List<string> errors = new List<string>();
				foreach(var value in ModelState.Values)
				{
					foreach (var error in value.Errors) 
					{
						errors.Add(error.ErrorMessage);
					}
				}
                string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
            }

            return View(product);
        }


		public async Task<IActionResult> Edit(int id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            return View(product);
		}

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            var existed_product = _dataContext.Products.Find(product.Id);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "_");
               
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "image/shop");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    existed_product.Image = imageName;

                    //delete old image
                    string oldfilePath = Path.Combine(uploadDir, product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while  deleting image");
                    }
                }

                //update other product properties

                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.BrandId = product.BrandId;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;

                _dataContext.Update(existed_product);

				await _dataContext.SaveChangesAsync();

				TempData["success"] = "Update product successfully";

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
            return View(product);
        }
        public async Task<IActionResult> Remove(int id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "image/shop");
            string oldfilePath = Path.Combine(uploadDir, product.Image);
            try
            {
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while  deleting image");
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index", "Admin/Product");
        }
    }
}
