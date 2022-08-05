using EmployeeManagement.Models.Users;
using EmployeeManagement.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, City = model.City };
                var identityResult = await userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var comfirmEmailLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, userToken = confirmEmailToken }, Request.Scheme);

                    Console.WriteLine(comfirmEmailLink);
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Super Admin"))
                    {
                        return RedirectToAction("listusers", "administration");
                    }
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "employees");
                    ViewBag.ErrorTitle = "Registration successful !";
                    ViewBag.ErrorMessage = "Please check your email for verification !";
                    return View("Error");
                }
                else
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string userToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorTitle = $"User with Id {userId} not found";
                return View("NotFound");
            }
            var result = await userManager.ConfirmEmailAsync(user, userToken);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "Registration failed";
            ViewBag.ErrorMessage = "Please contact Sudhanshu.s@ventla.io";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "employees");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> Login(string returnUrl)
        {
            var loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalAuthenticationSchemes = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null && !user.EmailConfirmed && await userManager.CheckPasswordAsync(user, loginViewModel.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(loginViewModel);
                }
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                    loginViewModel.RememberMe, false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return LocalRedirect(returnUrl);
                    else
                        return RedirectToAction("index", "employees");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var callBackUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });

            var externalAuthenticationProperties = signInManager.ConfigureExternalAuthenticationProperties(provider, callBackUrl);

            return new ChallengeResult(provider, externalAuthenticationProperties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalAuthenticationSchemes = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, remoteError);
                return View("login", loginViewModel);
            }

            var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();

            if (externalLoginInfo != null)
            {
                var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var externalUserEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                    if (!string.IsNullOrEmpty(externalUserEmail))
                    {
                        var externalUser = await userManager.FindByEmailAsync(externalUserEmail);

                        if (externalUser == null)
                        {
                            externalUser = new ApplicationUser()
                            {
                                Email = externalUserEmail,
                                UserName = externalUserEmail
                            };
                            await userManager.CreateAsync(externalUser);
                        }

                        await userManager.AddLoginAsync(externalUser, externalLoginInfo);
                        await signInManager.SignInAsync(externalUser, isPersistent: false);

                    }
                    return LocalRedirect(returnUrl);
                }
            }
            ViewBag.ErrorTitle = $"Email verification failed with the external login provider";
            ViewBag.ErrorMessage = $"Please contact sudhanshu.s@ventla.io";
            return View("Error");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<JsonResult> IsEmailInUse(string email)
        {
            var identityResult = await userManager.FindByEmailAsync(email);

            if (identityResult == null)
                return Json(true);

            else
            {
                string message = $"Email {email} is already in use";
                return Json(message);
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
