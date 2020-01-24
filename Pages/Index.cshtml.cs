using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace WebShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly WebDBContext _database;
        public Product[] mas { get; set; }
        public IndexModel(WebDBContext database)
        {
            _database = database;
        }

        public void OnGet(int genreId)
        {
            if (genreId != 0)
            {
                var temp = _database.ProductGenre.Where(x => x.IdGenre == genreId);
                mas = _database.Product.Join(temp, p => p.Id, c => c.IdProduct, (p, c) => new Product() { Id = p.Id, Name = p.Name, Cost = p.Cost, Quantity = p.Quantity, Description = p.Description, IconUrl = p.IconUrl, PictureUrl = p.PictureUrl, IdDeveloper = p.IdDeveloper, IdPublisher = p.IdPublisher }).ToArray();
            }
            else
            {
                mas = _database.Product.ToArray();
            }
            if (User.Identity.IsAuthenticated)
            {
                if (_database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id == 1)
                {
                    HttpContext.Session.SetString("IsRole", "Admin");
                }
                else
                {
                    HttpContext.Session.SetString("IsRole", "User");
                }
            }
            else
            {
                HttpContext.Session.SetString("IsRole", "Guest");
            }
        }

        public void OnGetRand()
        {
            mas = _database.Product.ToArray();
            mas = RandList(mas);

            var len = new Random().Next(1, mas.Length - 1);
            Product[] temp = new Product[len];
            Array.Copy(mas, 0, temp, 0, len);
            mas = new Product[len];
            Array.Copy(temp, mas, len);

        }

        public Product[] RandList(Product[] products)
        {
            for (int i = products.Length - 1; i >= 1; i--)
            {
                int j = new Random().Next(i + 1);
                // обменять значения data[j] и data[i]
                var temp = products[j];
                products[j] = products[i];
                products[i] = temp;
            }
            return products;
        } 

        public void OnPostSearch(string search)
        {
            //var temp = _database.Product.Where(x => x.Name == search);
            var temp_name = _database.Product.Where(p => EF.Functions.Like(p.Name, "%" +search+"%"));
            var temp_description = _database.Product.Where(p => EF.Functions.Like(p.Description, "%" + search + "%"));
            mas = temp_name.Union(temp_description).ToArray().Distinct().ToArray();
            //mas = _database.Product.Join(temp, p => p.Id, c => c.Id, (p, c) => new Product() { Id = p.Id, Name = p.Name, Cost = p.Cost, Quantity = p.Quantity, Description = p.Description, IconUrl = p.IconUrl, PictureUrl = p.PictureUrl, IdDeveloper = p.IdDeveloper, IdPublisher = p.IdPublisher }).ToArray();
        }

        public void OnGetAddBasket(int id)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            _database.Basket.Add(new Basket { IdProduct = (short)id, IdUser = userId, Status = 1 });
            _database.SaveChanges();
            mas = _database.Product.ToArray();
        }

        public bool InBasket(int id)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            if (_database.Basket.Where(x => x.IdUser == userId && x.IdProduct == id).FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }

        public void OnGetDelBasket(int id)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            _database.Basket.Remove(_database.Basket.Where(x => x.IdUser == userId && x.IdProduct == id).FirstOrDefault());
            _database.SaveChanges();
            mas = _database.Product.ToArray();
        }

        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }

    }
}
