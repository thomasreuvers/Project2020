using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Encryption;
using Project.Entities;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {

            return View();
        }

        #region Testing DB Connection and custom authentication

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("emptyModel", "Model is empty");
                return RedirectToAction("Index", "Home");
            }

            // Get user by email if not null
            var user = await _userService.GetUserByEmailTask(model.EmailAddress);
            if (user == null)
            {
                ModelState.AddModelError("userIsNull", "The user doesn't exist or could not be retrieved from the Db.");
                return RedirectToAction("Index", "Home");
            }

            // Validate entered password against stored hashed and salted password
            var cryptographyProcessor = new CryptographyProcessor();
            var isValidUser = cryptographyProcessor.VerifyHashedPassword(model.Password, user.PasswordHash, user.Salt);
            if (!isValidUser)
            {
                return RedirectToAction("Index", "Home");
            }


            /* Create the identity
             * Add other user info later on
             */
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.EmailAddress));

            // Add roles
            foreach (var role in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            // Sign in
            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
