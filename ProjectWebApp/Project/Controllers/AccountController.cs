using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Encryption;
using Project.Entities;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        // private readonly CryptographyProcessor _cryptographyProcessor;

        public AccountController(UserService userService)
        {
            _userService = userService;
            // _cryptographyProcessor = cryptographyProcessor;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var _cryptographyProcessor = new CryptographyProcessor();

               var salt = _cryptographyProcessor.CreateSalt();
               var hashedPassword = _cryptographyProcessor.GenerateHash(model.Password, salt);

               var doesUserExist = _userService.GetUserByEmail(model.EmailAddress) != null;

               // Create user & seed to database
               if (!doesUserExist)
               {
                   _userService.CreateUser(new User
                   {
                       EmailAddress = model.EmailAddress,
                       Username = model.Username,
                       PasswordHash = hashedPassword,
                       EmailIsVerified = false,
                       Salt = salt,
                       SecretUserKey = "0"
                   });
               }

               /* TODO:
                * Send verification email to user
                * Make a random SecretUserKey
                *
                */

               // test out hashing processor
            }

            return View();
        }
    }
}
