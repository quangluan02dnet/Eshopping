﻿using Microsoft.AspNetCore.Mvc;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;
using System.Diagnostics;

namespace Shopping_tutorial.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index()
        {
            var products = _dataContext.Products.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
           
        }
        public IActionResult RedirectToHome()
        {
            return Redirect("~/");
        }

		public IActionResult RedirectToAdmin()
		{
			return Redirect("~/admin");
		}
	}
}
