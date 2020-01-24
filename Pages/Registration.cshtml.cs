using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace WebShop
{
    public class RegistrationModel : PageModel
    {
        private readonly WebDBContext _database;

        [BindProperty]
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        public RegistrationModel(WebDBContext database)
        {
            _database = database;
        }

        public IActionResult OnPostRegister()
        {
            if (!ModelState.IsValid)
            {
                User user = _database.User.FirstOrDefault(u => u.EMail == EMail);
                int lastId = _database.User.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                if (user == null)
                {
                    // добавляем пользователя в бд
                    _database.User.Add(new User { Id = lastId + 1, EMail = EMail, Name = Name, Password = Password });
                    _database.SaveChanges();

                    Authenticate(EMail); // аутентификация
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return RedirectToPage("/Index");
        }

        private Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}