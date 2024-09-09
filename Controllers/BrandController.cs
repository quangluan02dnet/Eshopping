﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            BrandModel brand = _dataContext.Brands.Where(p => p.Slug == Slug).FirstOrDefault();
			
			if (brand == null) return RedirectToAction("Index");

			var productsByBrand = _dataContext.Products.Where(p => p.BrandId == brand.Id);

			return View(await productsByBrand.OrderByDescending(p => p.Id).ToListAsync());
		}
    }
}
