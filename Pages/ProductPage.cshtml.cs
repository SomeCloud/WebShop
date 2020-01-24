using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;

namespace WebShop
{
    public class ProductPageModel : PageModel
    {
        private readonly WebDBContext _database;
        public Product product { get; set; }
        public string genre { get; set; }
        public string publisher { get; set; }
        public string developer { get; set; }
        public string quantity { get; set; }
        public int _desc { get; set; }


        public Dictionary<int, string> Users = new Dictionary<int, string>();

        public Dictionary<int, string> Comments = new Dictionary<int, string>();

        public Dictionary<int, string> Dates = new Dictionary<int, string>();

        public ProductPageModel(WebDBContext database)
        {
            _database = database;
        }
        public void OnGet(int id, int desc)
        {
            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            var tempGenres = _database.ProductGenre.Join(_database.Genre, p => p.IdGenre, c => c.Id, (p, c) => new { c.Name, p.IdProduct});
            publisher = _database.Publisher.Where(x => x.Id == product.IdPublisher).FirstOrDefault().Name;
            developer = _database.Developer.Where(x => x.Id == product.IdDeveloper).FirstOrDefault().Name;
            foreach (var e in tempGenres)
            {
                if (e.IdProduct == id)
                {
                    genre += e.Name + ", ";
                }
            }
            quantity = GetQuantity();
            GetComments(id);
            _desc = desc;
            ViewData["Title"] = product.Name;
        }


        public bool InBasket(int id)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            if(_database.Basket.Where(x => x.IdUser == userId && x.IdProduct == id).FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }
        public void GetComments(int id)
        {
            var temp = _database.Comments.Where(x => x.IdProduct == id);  //Массив из комментариев только под одним тайтлом
            var temp2 = temp.Join(_database.Comment, p => p.IdCommentClientComment, c => c.Id, (p, c) => new { c.Text, c.Date, p.IdCommentClientComment, p.IdUserClientComment });
            var temp3 = temp2.Join(_database.User, p => p.IdUserClientComment, c => c.Id, (p, c) => new { c.Name, p.Text, p.Date, p.IdCommentClientComment });
            foreach (var e in temp3)
            {
                Dates.Add(e.IdCommentClientComment, e.Date.ToString());
                Comments.Add(e.IdCommentClientComment, e.Text);
                Users.Add(e.IdCommentClientComment, e.Name);
            }
        }

        private void sentComment(int id, string text)
        {
            int lastId = _database.Comment.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            _database.Comment.Add(new Comment { Id = Convert.ToInt16(lastId + 1), Text = text, Date = DateTime.Now });
            _database.Comments.Add(new Comments { IdProduct = (short)id, IdUserClientComment = Convert.ToInt16(_database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id), IdCommentClientComment = Convert.ToInt16(lastId + 1)});
            _database.SaveChanges();
        }

        public void OnPostComment(string comment, int id)
        {

            sentComment(id, comment);

            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            var tempGenres = _database.ProductGenre.Join(_database.Genre, p => p.IdGenre, c => c.Id, (p, c) => new { c.Name, p.IdProduct });
            publisher = _database.Publisher.Where(x => x.Id == product.IdPublisher).FirstOrDefault().Name;
            developer = _database.Developer.Where(x => x.Id == product.IdDeveloper).FirstOrDefault().Name;
            foreach (var e in tempGenres)
            {
                if (e.IdProduct == id)
                {
                    genre += e.Name + ", ";
                }
            }
            quantity = GetQuantity();
            GetComments(id);
            _desc = 3;
        }
        public string GetDate()
        {
            return DateTime.Now.ToString();
        }
        public string GetQuantity()
        {
            if (product.Quantity > 500)
            {
                return "очень много";
            }
            else if (product.Quantity > 250 && product.Quantity< 500)
            {
                return "много";
            }
            else if (product.Quantity > 150 && product.Quantity< 250)
            {
                return "достаточно";
            }
            else
            {
                return "мало";
            }
        }


        public void OnGetAddBasket(int id, int desc)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            _database.Basket.Add(new Basket { IdProduct = (short)id, IdUser = userId, Status = 1 });
            _database.SaveChanges();

            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            var tempGenres = _database.ProductGenre.Join(_database.Genre, p => p.IdGenre, c => c.Id, (p, c) => new { c.Name, p.IdProduct });
            publisher = _database.Publisher.Where(x => x.Id == product.IdPublisher).FirstOrDefault().Name;
            developer = _database.Developer.Where(x => x.Id == product.IdDeveloper).FirstOrDefault().Name;
            foreach (var e in tempGenres)
            {
                if (e.IdProduct == id)
                {
                    genre += e.Name + ", ";
                }
            }
            quantity = GetQuantity();
            GetComments(id);
            _desc = desc;
            ViewData["Title"] = product.Name;
        }


        public void OnGetDelBasket(int id, int desc)
        {
            int userId = _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Id;
            _database.Basket.Remove(_database.Basket.Where(x => x.IdUser == userId && x.IdProduct == id).FirstOrDefault());
            _database.SaveChanges();
            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            var tempGenres = _database.ProductGenre.Join(_database.Genre, p => p.IdGenre, c => c.Id, (p, c) => new { c.Name, p.IdProduct });
            publisher = _database.Publisher.Where(x => x.Id == product.IdPublisher).FirstOrDefault().Name;
            developer = _database.Developer.Where(x => x.Id == product.IdDeveloper).FirstOrDefault().Name;
            foreach (var e in tempGenres)
            {
                if (e.IdProduct == id)
                {
                    genre += e.Name + ", ";
                }
            }
            quantity = GetQuantity();
            GetComments(id);
            _desc = desc;
            ViewData["Title"] = product.Name;
        }
        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }
    }
}