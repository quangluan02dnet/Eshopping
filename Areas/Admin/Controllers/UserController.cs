using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.Include(o => o.Role).OrderByDescending(u => u.Id).ToListAsync());
        }

		public async Task<IActionResult> Add()
		{
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(new AppUserModel());
		}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,AppUserModel user)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var existedUser = await _userManager.FindByIdAsync(id);
            if (existedUser == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                existedUser.UserName = user.UserName;
                existedUser.Email = user.Email;
                existedUser.RoleId = user.RoleId;
                existedUser.PhoneNumber = user.PhoneNumber;
                var updateUser = await _userManager.UpdateAsync(existedUser);
                if (!updateUser.Succeeded)
                {
                    AddIdentityErrors(updateUser);
                    return View(user);
                }
                TempData["success"] = "Cập nhật người dùng thành công !!!";
                return RedirectToAction("Index", "User");
            }          
            
          
            var roles = await _roleManager.Roles.ToListAsync();
          
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Add(AppUserModel user)
        {
            if (ModelState.IsValid)
            {               
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (result.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email);

                    var userId = createUser.Id;

                    var _role = _roleManager.FindByIdAsync(user.RoleId);

                    var addToRoleResult = await _userManager.AddToRoleAsync(createUser, _role.Result.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(String.Empty, error.Description);
                        }
                    }
                    TempData["success"] = "Thêm thành công !!!";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    var roles = await _roleManager.Roles.ToListAsync();
                    ViewBag.Role = new SelectList(roles, "Id", "Name");
                    AddIdentityErrors(result);
                    return View(result);
                }
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

            var role = await _roleManager.Roles.ToListAsync();
            ViewBag.Role = new SelectList(role, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                var deleteUserResult = await _userManager.DeleteAsync(user);
                if (deleteUserResult.Succeeded)
                {
                    TempData["success"] = "Đã xóa thành công !!!";
                    return RedirectToAction("Index", "User");
                }
                
                return View("Erorr");
            }
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }
        public IActionResult Logout()
        {
            return Redirect("~/Account/Logout");
        }
    }
}
