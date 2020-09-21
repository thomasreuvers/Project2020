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
            if (ModelState.IsValid)
            {
                // Get user by email if not null
                var user = await _userService.GetUserByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    ModelState.AddModelError("userIsNull", "The user doesn't exist or could not be retrieved from the Db.");
                    return View();
                }

                // Validate entered password against stored hashed and salted password
                var cryptographyProcessor = new CryptographyProcessor();
                var isValidUser = cryptographyProcessor.VerifyHashedPassword(model.Password, user.PasswordHash, user.Salt);
                if (!isValidUser)
                {
                    ModelState.AddModelError("userIsNull", "Password or Email is invalid!");
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

                // Sign in
                var principle = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
            }
            return View();
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
            if (ModelState.IsValid)
            { 
                var _cryptographyProcessor = new CryptographyProcessor();

               var salt = _cryptographyProcessor.CreateSalt();
               var hashedPassword = _cryptographyProcessor.GenerateHash(model.Password, salt);

               var doesUserExist = await _userService.GetUserByEmailAsync(model.EmailAddress) != null;

               // Create user & seed to database
               if (!doesUserExist)
               {
                   var user = await _userService.CreateUserAsync(new User
                   {
                       EmailAddress = model.EmailAddress,
                       Username = model.Username,
                       PasswordHash = hashedPassword,
                       EmailIsVerified = false,
                       Salt = salt,
                       SecretUserKey = "".RandomString(),
                       VerificationToken = "".RandomString()
                   });

                   _mailService.SendMailAsync(model.EmailAddress, "test", $"https://localhost:44325/Account/verifyemailaddress/?id={user.Id}&verificationtoken={user.VerificationToken}");
               }

               /* TODO:
                * Send proper verification email to user
                *
                */
            }

            return View();
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

                    // Show user email verified screen.

                    return View("Login");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
