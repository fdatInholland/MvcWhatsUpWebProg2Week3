using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models;
using MvcWhatsUp.Services.Interfaces;
using System.Security.Claims;

namespace MvcWhatsUp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            User? user = _usersService.GetByLoginCredentials(loginModel.Username, loginModel.Password);

            if (user is null)
            {
                ViewBag.ErrorMessage = "Bad username/password combination";

                return View(loginModel);
            }
            else
            {
                SignInUser(user);
                //HttpContext.Session.SetObject("LoggedInUser", user);
                return RedirectToAction("Index", "Users");
            }
        }

        private void SignInUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            HttpContext.SignInAsync(claimsPrincipal);
        }


        [HttpGet]
        public IActionResult Logoff()
        {
            HttpContext.SignOutAsync();

            HttpContext.Session.Remove("LoggedInUser");

            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
