using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;
using Shopping_tutorial.Models.ViewModels;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext; 
		public OrderController(DataContext dataContext)
		{
			_dataContext = dataContext;

		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Orders.OrderByDescending(o => o.CreatedDate).ToListAsync());
		}
        public async Task<IActionResult>ViewOrder(string OrderCode)
        {
            OrderItemViewModel orderItemViewModel = new OrderItemViewModel();

            orderItemViewModel.Order = _dataContext.Orders.Where(x => x.OrderCode == OrderCode).FirstOrDefault();

            orderItemViewModel.Items = await _dataContext.OrderDetails
                .Include(o => o.Product).Where(o => o.OrderCode == OrderCode)
                .ToListAsync();

            return View(orderItemViewModel);
        }

        public async Task<IActionResult> Approve(string OrderCode)
        {
            if (OrderCode == null)
            {
                TempData["errorr"] = "Có lỗi xảy ra";
                return RedirectToAction("Index", "Order");
            }
            var order = _dataContext.Orders.Where(x => x.OrderCode == OrderCode).FirstOrDefault();
            order.Status = 2;
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã duyệt đơn hàng thành công.";
            return RedirectToAction("Index", "Order");
        }


        public async Task<IActionResult> Reject(string OrderCode)
        {
            if (OrderCode == null)
            {
                TempData["errorr"] = "Có lỗi xảy ra";
                return RedirectToAction("Index", "Order");
            }
            var order = _dataContext.Orders.Where(x => x.OrderCode == OrderCode).FirstOrDefault();
            order.Status = 3;
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã từ chối đơn hàng !!!";
            return RedirectToAction("Index", "Order");
        }
    }
}
