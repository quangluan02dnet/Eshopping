using Microsoft.AspNetCore.Mvc;
using Shopping_tutorial.Models;
using Shopping_tutorial.Models.ViewModels;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>> ("Cart") ?? new List<CartItemModel> ();
            CartItemViewModel cartItemViewModel = new CartItemViewModel()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };
            return View(cartItemViewModel);
        }
        public ActionResult Checkout()
        {
            return View("~/View/Checkout/Index.cshtml");
        }
        public async Task<IActionResult>Add(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(x => x.ProductId == Id).FirstOrDefault();
            if(cartItem == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công !!!";

            HttpContext.Session.SetJson("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString());
        }

		public async Task<IActionResult>Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			CartItemModel cartItem = cart.Where(x => x.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
                --cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(x => x.ProductId == Id);
			}

            if (cart.Count == 0)
            {
				HttpContext.Session.Remove("Cart");
			}
            else
            {
				HttpContext.Session.SetJson("Cart", cart);
			}

            TempData["success"] = "Đã giảm số lượng !!!";

            return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			CartItemModel cartItem = cart.Where(x => x.ProductId == Id).FirstOrDefault();

            if (cartItem != null)
            {
				++cartItem.Quantity;
			}
            TempData["success"] = "Đã tăng số lượng !!";

            HttpContext.Session.SetJson("Cart", cart);

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			CartItemModel cartItem = cart.Where(x => x.ProductId == Id).FirstOrDefault();

			if (cartItem != null)
			{
				cart.Remove(cartItem);
			}
            TempData["success"] = "Đã xóa sản phẩm !!!";

            HttpContext.Session.SetJson("Cart", cart);

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<IActionResult> Clear()
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			cart.Clear();

			TempData["success"] = "Đã xóa toàn bộ sản phẩm !!!";

			HttpContext.Session.Remove("Cart");

			return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}
