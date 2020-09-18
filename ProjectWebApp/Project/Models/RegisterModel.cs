using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Custom)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match, Type again!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Compare("EmailAddress", ErrorMessage = "Emails don't match, Type again!")]
        public string ConfirmEmailAddress { get; set; }
    }
}
