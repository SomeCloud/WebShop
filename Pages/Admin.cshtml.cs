using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebShop
{
    [Authorize]
    public class AdminModel : PageModel
    {
        private readonly WebDBContext _database;

        public User[] Users;
        public Product[] Products;
        public Dictionary<int, string> Developers = new Dictionary<int, string>();

        public Dictionary<int, string> Publishers = new Dictionary<int, string>();
        public AdminModel(WebDBContext database)
        {
            _database = database;
        }

        public string _desc { get; set; }

        public void OnGet(string desc)
        {
            Users = _database.User.ToArray();
            Products = _database.Product.ToArray();
            var _developers = _database.Developer.ToArray();
            var _publisher = _database.Publisher.ToArray();
            foreach (var e in _developers)
            {
                Developers.Add(e.Id, e.Name);
            }
            foreach (var e in _publisher)
            {
                Publishers.Add(e.Id, e.Name);
            }
            _desc = desc;
        }

        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }

        public void OnGetDeleteUser(int id)
        {
            User user = _database.User.Where(x => x.Id == id).FirstOrDefault();
            _database.Remove(user);
            _database.SaveChanges();
            OnGet("users");
        }

        public void OnGetDeleteProduct(int id)
        {
            Product product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            _database.Remove(product);
            _database.SaveChanges();
            OnGet("products");
        }

    }
}