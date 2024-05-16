using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
			_userManager = userManager;
			_roleManager = roleManager;
            _mapper = mapper;
        }


		public async Task<IActionResult> Index(string searchInp)
		{
			if (string.IsNullOrEmpty(searchInp))
			{
				var users = await _userManager.Users.Select(u => new UserViewModel()
				{
					Id = u.Id,
					FName = u.FName,
					LName = u.LName,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
					Roles =  _userManager.GetRolesAsync(u).Result
				}).ToListAsync();

				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(searchInp);
				if (user != null)
				{
					var mappedUser = new UserViewModel
					{
						Id = user.Id,
						FName = user.FName,
						LName = user.LName,
						Email = user.Email,
						PhoneNumber = user.PhoneNumber,
						Roles = _userManager.GetRolesAsync(user).Result
					};
					return View(new List<UserViewModel> { mappedUser });
				}
			}

			return View(Enumerable.Empty<UserViewModel>());
		}

		//*************************************************************
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
			var user = await _userManager.FindByIdAsync(id);

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);

            if (mappedUser == null)
            {
                return NotFound();
            }
            return View(viewName, mappedUser);
        }

        //*************************************************************
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(UserVM.Id);
                    if(user != null)
                    {
                        user.FName = UserVM.FName;
                        user.LName = UserVM.LName;
                        user.PhoneNumber = UserVM.PhoneNumber;

                        await _userManager.UpdateAsync(user);

                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(UserVM);
        }

        //**************************************************************
        public Task<IActionResult> Delete(string? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id); 

                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Error", "Home");
            }
        }
    }
}
