using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле Email должно быть заполнено")]
        //[StringLength(20, MinimumLength = 6)] //_@_.ii
        [EmailAddress(ErrorMessage = "Поле Email должно быть заполнено")]
        [Remote("CheckEmail", "Account", ErrorMessage = "Current email already exist")]
        public string Email { get; set; }

        [Required]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[.#$^+=!*()@%&]).{8,}$", 
        //    ErrorMessage = "Password must contain blablabla")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "123123")]
        public string PasswordConfirmation { get; set; }

        public string FullName { get; set; }

        [Range(1, 2)]
        public string Value { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string SecondName { get; set; } 
        //public string Patronymic { get; set; } 


        [Required]
        //[RegularExpression("^[0-9*#+-()]+$")]
        [Phone]
        [CreditCard]
        public string PhoneNumber { get; set; }
    
    }
}
