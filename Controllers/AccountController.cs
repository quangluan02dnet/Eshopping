using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping_tutorial.Models;
using Shopping_tutorial.Models.ViewModels;

namespace Shopping_tutorial.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly SignInManager<AppUserModel> _signInManager;


        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login(string url)
		{
			return View(new LoginViewModel { ReturnUrl = url });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel userModel)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(userModel.UserName, userModel.Password, false, false);
				if (signInResult.Succeeded)
				{
					TempData["success"] = "Log in successfully !!!";
					return Redirect(userModel.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Invalid username or password");
			}
			return View(userModel);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserModel userModel)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = userModel.UserName, Email = userModel.Email};

				IdentityResult identityResult = await _userManager.CreateAsync(newUser, userModel.Password);

				if (identityResult.Succeeded)
				{
					var createUser = await _userManager.FindByEmailAsync(newUser.Email);
					
					await _userManager.AddToRoleAsync(createUser, "User");

					TempData["success"] = "Register successfully !!!";
					return Redirect("Login");
				}
				else
				{
					foreach (IdentityError identityError in identityResult.Errors)
					{
						ModelState.AddModelError("", identityError.Description);
					}
				}				
			}
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("/");
		}
	}
}
