using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        //******************************************************************
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel RoleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(RoleVM);

                await _roleManager.CreateAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }

            return View(RoleVM);
        }

        //******************************************************************
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    RoleName = r.Name,
                }).ToListAsync();

                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                if (role != null)
                {
                    var mappedRole = new RoleViewModel
                    {
                        Id = role.Id,
                        RoleName= role.Name,
                    };
                    return View(new List<RoleViewModel> { mappedRole });
                }
            }

            return View(Enumerable.Empty<RoleViewModel>());
        }

        //*************************************************************
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var role = await _roleManager.FindByIdAsync(id);

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            if (mappedRole == null)
            {
                return NotFound();
            }
            return View(viewName, mappedRole);
        }

        //*************************************************************
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel RoleVM)
        {
            if (id != RoleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role != null)
                    {
                        role.Name = RoleVM.RoleName;

                        await _roleManager.UpdateAsync(role);

                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(RoleVM);
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
                var role = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(role);

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
