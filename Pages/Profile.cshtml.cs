using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebShop
{
    [Authorize]
    public class ProfileModel : PageModel
    {

        private WebDBContext _database;
        public Product[] mas { get; set; }
        public ProfileModel(WebDBContext database)
        {
            _database = database;
        }
        public void OnGet(int genreId)
        {
            ViewData["Title"] = GetUserName();
            if (genreId != 0)
            {
                var temp = _database.ProductGenre.Where(x => x.IdGenre == genreId);
                mas = _database.Product.Join(temp, p => p.Id, c => c.IdProduct, (p, c) => new Product() { Id = p.Id, Name = p.Name, Cost = p.Cost, Quantity = p.Quantity, Description = p.Description, IconUrl = p.IconUrl, PictureUrl = p.PictureUrl, IdDeveloper = p.IdDeveloper, IdPublisher = p.IdPublisher }).ToArray();
            }
            else
            {
                mas = _database.Product.ToArray();
            }
        }

        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }

        
        public void OnPostChangName(string name)
        {
            User user = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault();
            if (name != null)
            {
                if (_database.User.Any(p => p.Name == name) == false)
                {
                    user.Name = name;
                    _database.SaveChanges();
                }
            }

        }

        public void OnPostChangEMail(string email)
        {
            ViewData["Title"] = GetUserName();
            User user = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault();
            if (email != null)
            {
                if (_database.User.Any(p => p.EMail == email) == false)
                {
                    user.EMail = email;
                    _database.SaveChanges();
                    Authenticate(email);
                }
            }
        }

        public void OnPostChangPassword(string password, string confirmPassword)
        {

            User user = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault();
            if (password == confirmPassword)
            {
                if (password != null)
                {
                    if (user.Password != password)
                    {
                        user.Password = password;
                        _database.SaveChanges();
                    }
                }
            }
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