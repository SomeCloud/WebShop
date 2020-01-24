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
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        private readonly WebDBContext _database;
        public LoginModel(WebDBContext database)
        {
            _database = database;
        }


        [Required(ErrorMessage = "Не указан Email")]
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPostLogin()
        {
            if (ModelState.IsValid)
            {
                User user = _database.User.FirstOrDefault(u => u.EMail == EMail && u.Password == Password);
                if (user != null)
                {
                    Authenticate(EMail); // аутентификация  
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return RedirectToPage("/Login");
                }
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                return RedirectToPage("/Login");
            }
        }
        public void OnGet()
        {
            
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

        public IActionResult OnGetLogout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }

    }
}