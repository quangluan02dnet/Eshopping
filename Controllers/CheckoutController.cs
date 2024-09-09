using Microsoft.AspNetCore.Mvc;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;
using System.Security.Claims;

namespace Shopping_tutorial.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        public CheckoutController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
		public async Task<IActionResult> Checkout()
		{
            var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (UserEmail == null)
            {
                return Redirect("Login");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = orderCode;
                orderItem.UserName = UserEmail;
                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var item in cartItems)
                {
                    var orderDetails = new OrderDetails();
					orderDetails.UserName = UserEmail;
                    orderDetails.Price = item.Price;
                    orderDetails.Quantity = item.Quantity;
                    orderDetails.OrderCode = orderCode;
                    orderDetails.ProductId = item.ProductId;
                    _dataContext.Add(orderDetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Checkout thành công, vui lòng đợi duyệt đơn !!!";
                return RedirectToAction("Index", "Cart");
			}
            return View();
        }
    }
}
