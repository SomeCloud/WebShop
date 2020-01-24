using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebShop.Models;
using System.IO;

namespace WebShop
{
    public class EditProductModel : PageModel
    {
        private readonly WebDBContext _database;

        public Product product;
        public string codes;
        public string genres;
        public EditProductModel(WebDBContext database)
        {
            _database = database;
        }

        private string getCodesStr(int id)
        {
            var _codes = _database.Code.Where(x => x.IdProduct == id).ToArray();
            string[] arrcodes = new string[_codes.Count()];
            for (int i = 0; i < arrcodes.Count(); i++)
            {
                arrcodes[i] = _codes[i].Key;
            }
            return string.Join("\n", arrcodes);
        }

        private string getGenresStr(int id)
        {
            var Genres = _database.ProductGenre.Join(_database.Genre, p => p.IdGenre, c => c.Id, (p, c) => new { c.Name, p.IdProduct }).ToArray();
            string[] arrgenres = new string[Genres.Count()];
            for (int i = 0; i < arrgenres.Count(); i++)
            {
                if (Genres[i].IdProduct == id)
                {
                    arrgenres[i] = Genres[i].Name;
                }
            }
            arrgenres = arrgenres.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return string.Join("\n", arrgenres);
        }

        public void OnGet(int id)
        {
            if (id == 0)
            {
                var lastId = _database.Product.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                _database.Product.Add(new Product { Id = (short)(lastId + 1), Name = "Новый товар", Cost = 0, Description = "Новое описание", IconUrl = "", PictureUrl = "", SystemRequirements = "Новые системные требования", IdPublisher = 0, IdDeveloper = 0 });
                _database.SaveChanges();
                id = (short)(lastId + 1);
            }
            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            codes = getCodesStr(id);
            genres = getGenresStr(id);

        }

        public void OnPostSave(string name, string cost, string description, string iconUrl, string pictureUrl, string systemRequirements, string codes, string genres, short id)
        {
            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            if (name != null)
            {
                product.Name = name;
            }
            if (cost != null && cost.All(char.IsDigit))
            {
                product.Cost = Convert.ToInt16(cost);
            }
            if (description != null)
            {
                product.Description = description;
            }
            if (iconUrl != null)
            {
                product.IconUrl = iconUrl;
            }
            if (pictureUrl != null)
            {
                product.PictureUrl = pictureUrl;
            }
            if (systemRequirements != null)
            {
                product.SystemRequirements = systemRequirements;
            }
            if (codes != null)
            {
                var codesarr = codes.Split(new char[] { '\r', '\n' });
                codesarr = codesarr.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();              

                foreach (var e in codesarr)
                {
                    if (_database.Code.Any(p => p.Key == e) == false)
                    {
                        int lastId = _database.Code.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                        _database.Code.Add(new Code { IdProduct = id, Key = e, Id = (short)(lastId + 1) });
                        _database.SaveChanges();
                    }
                }
            }
            if (genres != null)
            {
                var genresarr = genres.Split(new char[] { '\r', '\n' });
                genresarr = genresarr.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                Dictionary<string, int> Genres = GetGenreDict();

                foreach (var e in genresarr)
                {
                    var ganreId = Genres[e];
                    if (_database.ProductGenre.Where(x => x.IdProduct == id && x.IdGenre == ganreId).FirstOrDefault() == null)
                    {
                        _database.ProductGenre.Add(new ProductGenre { IdProduct = id, IdGenre = (short)Genres[e] });
                        _database.SaveChanges();
                    }
                }
            }        
            product.Quantity = (short)_database.Code.Count(x => x.IdProduct == id);
            _database.SaveChanges();
            OnGet(id);
        }


        private Dictionary<string, int> GetGenreDict()
        {
            Dictionary<string, int> Genres = new Dictionary<string, int>();
            foreach (var e in _database.Genre.ToArray())
            {
                Genres.Add(e.Name, e.Id);
            }
            return Genres;
        }

    }
}