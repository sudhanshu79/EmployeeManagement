using EmployeeManagement.Models;
using EmployeeManagement.Models.Users;
using EmployeeManagement.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers.Aministration
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager
                                        , UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole() { Name = createRoleViewModel.RoleName };
                var identityResponse = await roleManager.CreateAsync(identityRole);

                if (identityResponse.Succeeded)
                {
                    return RedirectToAction("listroles");
                }
                else
                {
                    foreach (IdentityError error in identityResponse.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }
            return View(createRoleViewModel);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                var users = await userManager.GetUsersInRoleAsync(role.Name);

                var editRoleViewModel = new EditRoleViewodel()
                {
                    Id = id,
                    RoleName = role.Name,
                    Users = users.ToList()
                };
                return View(editRoleViewModel);
            }
        }

        [HttpPost]

        public async Task<IActionResult> EditRole(EditRoleViewodel editRoleViewodel)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(editRoleViewodel.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id:{editRoleViewodel.Id} not found";
                    return View("ErrorMessage");
                }
                else
                {
                    role.Name = editRoleViewodel.RoleName;
                    var identityRoleResult = await roleManager.UpdateAsync(role);

                    if (identityRoleResult.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    else
                    {
                        foreach (var error in identityRoleResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return View(editRoleViewodel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {RoleId} not found";
                return RedirectToAction("ErrorMessage");
            }

            var editUsersInRoleViewModel = new List<EditUsersInRoleViewModel>();

            foreach (var user in userManager.Users.ToList())
            {
                editUsersInRoleViewModel.Add(new EditUsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(editUsersInRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<EditUsersInRoleViewModel> editUsersInRoleViewModels, string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {RoleId} not found";
                return View("ErrorMessage");
            }

            foreach (var userInRole in editUsersInRoleViewModels)
            {
                IdentityResult identityResult = null;
                var user = await userManager.FindByIdAsync(userInRole.UserId);
                if (userInRole.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    identityResult = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userInRole.IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    identityResult = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (identityResult.Succeeded && userInRole == editUsersInRoleViewModels.LastOrDefault())
                    return RedirectToAction("EditRole", new { id = RoleId });
            }

            return RedirectToAction("EditRole", new { id = RoleId });
        }

        public IActionResult ListUsers()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                var roles = await userManager.GetRolesAsync(user);
                var claims = await userManager.GetClaimsAsync(user);
                var editUserViewModel = new EditUserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    City = user.City,
                    Roles = roles.ToList(),
                    Claims = claims.Select(x => new string(x.Type + " : " + x.Value)).ToList()
                };

                return View(editUserViewModel);

            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(editUserViewModel.Id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id:{editUserViewModel.Id} not found";
                    return View("ErrorMessage");
                }
                else
                {
                    user.UserName = editUserViewModel.UserName;
                    user.Email = editUserViewModel.Email;
                    user.City = editUserViewModel.City;

                    var identityResult = await userManager.UpdateAsync(user);

                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("listusers");
                    }
                    else
                    {
                        foreach (var error in identityResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ListUsers");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                try
                {
                    var identityResult = await roleManager.DeleteAsync(role);

                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    else
                    {
                        foreach (var error in identityResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("ListRoles");
                }
                catch (DbUpdateException)
                {
                    ViewBag.ErrorTitle = $"Unable to delete {role.Name} role as a user is associated with it.";
                    ViewBag.ErrorMessage = $"If you want to delete {role.Name} role, please delete the user first which is associated with this role.";
                    return View("error");
                }

            }
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]

        public async Task<IActionResult> EditRolesInUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                ViewBag.UserId = id;
                var editRolesInUserViewModel = new List<EditRolesInUserViewModel>();

                foreach (var role in roleManager.Roles.ToList())
                {
                    var isInRole = await userManager.IsInRoleAsync(user, role.Name);
                    editRolesInUserViewModel.Add(new EditRolesInUserViewModel()
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        IsSelected = isInRole
                    });
                }
                return View(editRolesInUserViewModel);
            }
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRolesInUser(List<EditRolesInUserViewModel> editRolesInUserViewModels, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                foreach (var editRoleUserViewModel in editRolesInUserViewModels)
                {
                    IdentityResult identityResult = null;
                    if (editRoleUserViewModel.IsSelected && !(await userManager.IsInRoleAsync(user, editRoleUserViewModel.RoleName)))
                    {
                        identityResult = await userManager.AddToRoleAsync(user, editRoleUserViewModel.RoleName);
                    }
                    else if (!editRoleUserViewModel.IsSelected && (await userManager.IsInRoleAsync(user, editRoleUserViewModel.RoleName)))
                    {
                        identityResult = await userManager.RemoveFromRoleAsync(user, editRoleUserViewModel.RoleName);
                    }
                    else
                    {
                        continue;
                    }
                    if (identityResult.Succeeded && editRoleUserViewModel == editRolesInUserViewModels.Last())
                    {
                        return RedirectToAction("EditUser", new { id = id });
                    }
                }
            }
            return RedirectToAction("EditUser", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> EditClaimsInUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                try
                {
                    ViewBag.UserId = id;
                    var claims = await userManager.GetClaimsAsync(user);

                    var userClaims = new List<EditClaimsInUserViewModel>();

                    foreach (var claim in ClaimsStore.AllClaims)
                    {
                        userClaims.Add(new EditClaimsInUserViewModel()
                        {
                            ClaimType = claim.Type,
                            IsSelected = claims.Any(x => x.Type == claim.Type && x.Value.Equals("true"))
                        });
                    }
                    return View(userClaims);
                }
                catch (System.Exception)
                {
                    return View("error");
                }

            }
        }

        [HttpPost]
        public async Task<IActionResult> EditClaimsInUser(List<EditClaimsInUserViewModel> viewModels, string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id:{id} not found";
                return View("ErrorMessage");
            }
            else
            {
                var claims = await userManager.GetClaimsAsync(user);

                var existingClaims = await userManager.GetClaimsAsync(user);
                var result = await userManager.RemoveClaimsAsync(user, existingClaims);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing claims");
                    return View(viewModels);
                }

                result = await userManager.AddClaimsAsync(user,
                                                         viewModels.Select(c => new System.Security.Claims.Claim(c.ClaimType, Convert.ToString(c.IsSelected).ToLower())));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected claims to user");
                    return View(viewModels);
                }
            }
            return RedirectToAction("EditUser", new { id = id });
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}
