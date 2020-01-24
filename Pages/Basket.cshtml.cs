using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebShop
{
    [Authorize]
    public class BasketModel : PageModel
    {
        private readonly WebDBContext _database;
        public Product[] mas { get; set; }
        public BasketModel(WebDBContext database)
        {
            _database = database;
        }
        public void OnGet()
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            var temp = _database.Basket.Where(x => x.IdUser == userId);
            mas = _database.Product.Join(temp, p => p.Id, c => c.IdProduct, (p, c) => new Product() { Id = p.Id, Name = p.Name, Cost = p.Cost, Quantity = p.Quantity, Description = p.Description, IconUrl = p.IconUrl, PictureUrl = p.PictureUrl, IdDeveloper = p.IdDeveloper, IdPublisher = p.IdPublisher }).ToArray();
        }

        public void OnGetDelBasket(int id)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            _database.Basket.Remove(_database.Basket.Where(x => x.IdUser == userId && x.IdProduct == id).FirstOrDefault());
            _database.SaveChanges();
            var temp = _database.Basket.Where(x => x.IdUser == userId);
            mas = _database.Product.Join(temp, p => p.Id, c => c.IdProduct, (p, c) => new Product() { Id = p.Id, Name = p.Name, Cost = p.Cost, Quantity = p.Quantity, Description = p.Description, IconUrl = p.IconUrl, PictureUrl = p.PictureUrl, IdDeveloper = p.IdDeveloper, IdPublisher = p.IdPublisher }).ToArray();
        }

        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }

    }
}