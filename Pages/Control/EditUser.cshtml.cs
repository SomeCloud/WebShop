using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;

namespace WebShop
{
    public class EditUserModel : PageModel
    {
        private readonly WebDBContext _database;

        public User user;
        public EditUserModel(WebDBContext database)
        {
            _database = database;
        }
    
        public void OnGet(int id)
        {
            user = _database.User.Where(x => x.Id == id).FirstOrDefault();
        }

        public void OnPostSave(string name, string email, string password, int id)
        {
            user = _database.User.Where(x => x.Id == id).FirstOrDefault();
            if (name != "")
            {
                user.Name = name;
            }
            if (email != "")
            {
                user.EMail = email;
            }
            if (password != "")
            {
                user.Password = password;
            }
            _database.SaveChanges();
        }

    }
}