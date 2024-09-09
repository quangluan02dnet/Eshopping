using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Repository;

namespace Shopping_tutorial.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly DataContext _dataContext;
		public RoleController(DataContext dataContext, RoleManager<IdentityRole> roleManager )
		{
			_dataContext = dataContext;
            _roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync());
		}
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(string Id)
        {
            return View(await _roleManager.FindByIdAsync(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IdentityRole model)
        {
            var roleExists = await _roleManager.RoleExistsAsync(model.Name);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = model.Name
                });
                TempData["success"] = "Thêm quyền thàng công";
                return RedirectToAction("Index", "Role");                
            }
            TempData["error"] = "Quyền " + model.Name + " đã có trong DB";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            else
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Id);
                if (!roleExists)
                {
                    IdentityRole identityRole = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);

                    identityRole.Name = model.Name;

                    IdentityResult result = await _roleManager.UpdateAsync(identityRole).ConfigureAwait(false);

                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors.Select(x => x.Description));
                    }
                    else
                    {
                        TempData["success"] = "Đã chỉnh sửa quyền thành công";
                        return RedirectToAction("Index", "Role");
                    }                    
                }
                else
                {
                    TempData["error"] = "Quyền " + model.Name + " đã có trong DB";
                }                
            } 
            return View(model);
        }

        public async Task<IActionResult> Remove(IdentityRole model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            else
            {  
                IdentityRole identityRole = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);
                
                if (identityRole == null)
                {
                    TempData["error"] = "Ôi lỗi rồi !!!";
                }
                else
                {
                    IdentityResult result = await _roleManager.DeleteAsync(identityRole).ConfigureAwait(false);

                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors.Select(x => x.Description));
                    }
                    else
                    {
                        TempData["success"] = "Đã xóa thành công";
                        return RedirectToAction("Index", "Role");
                    }
                }
                    
                
            }
            return View(model);
        }
    }
}
	