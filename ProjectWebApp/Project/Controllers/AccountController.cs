using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Encryption;
using Project.Entities;
using Project.ExtensionMethods;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly MailService _mailService;

        public AccountController(UserService userService, MailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View();

            // Get user by email if not null
            var user = await _userService.GetUserByEmailAsync(model.EmailAddress);
            if (user == null)
            {
                ModelState.AddModelError("userIsNull", "The user doesn't exist or could not be retrieved from the Db.");
                return View();
            }

            // Validate entered password against stored hashed and salted password
            var isValidUser = CryptographyProcessor.VerifyHashedPassword(model.Password, user.PasswordHash, user.Salt);
            if (!isValidUser)
            {
                ModelState.AddModelError("invalidPasswordOrEmail", "Password or Email is invalid!");
                return View();
            }

            // Check if email is verified
            if (!user.EmailIsVerified)
            {
                ModelState.AddModelError("emailNotVerified", "Please verify your email!");
                return View();
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

            // Sign in and set cookie lifetime
            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddHours(1.0)
            });

            // Redirect to secured user-panel
            return RedirectToAction("Index", "Panel");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View();

            var salt = CryptographyProcessor.CreateSalt();
            var hashedPassword = CryptographyProcessor.GenerateHash(model.Password, salt);

            var doesUserExist = await _userService.GetUserByEmailAsync(model.EmailAddress) != null;

            // Create user & seed to database
            if (doesUserExist)
            {
                ModelState.AddModelError("userAlreadyExists", "This email is already in use, please use a different one!");
                return View();
            }

            var user = await _userService.CreateUserAsync(new User
            {
                EmailAddress = model.EmailAddress,
                Username = model.Username,
                PasswordHash = hashedPassword,
                EmailIsVerified = false,
                Salt = salt,
                SecretUserKey = CryptographyProcessor.GenerateKey(),
                VerificationToken = CryptographyProcessor.GenerateKey()
            });

            _mailService.SendMailAsync(model.EmailAddress, "test", $"https://localhost:44325/Account/verifyemailaddress/?id={user.Id}&verificationtoken={user.VerificationToken}");

            /* TODO:
                * Send proper verification email to user
                *
                */

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> VerifyEmailAddress(string id, string verificationToken)
        {
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(verificationToken))
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user != null && user.VerificationToken == verificationToken)
                {
                    user.EmailIsVerified = true;

                    _userService.UpdateUserAsync(user);

                    return View();
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
