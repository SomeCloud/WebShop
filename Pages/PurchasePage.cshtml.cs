using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using WebShop.Models;

namespace WebShop
{
    public class PurchasePageModel : PageModel
    {
        private readonly WebDBContext _database;
        public Product product { get; set; }
        public string key { get; set; }
        public PurchasePageModel(WebDBContext database)
        {
            _database = database;
        }

        public void OnGet(int id)
        {
            product = _database.Product.Where(x => x.Id == id).FirstOrDefault();
            key = _database.Code.Where(x => x.IdProduct == id).FirstOrDefault().Key;
            //_database.Code.Remove(_database.Code.Where(x => x.IdProduct == id).FirstOrDefault());
            if (User.Identity.IsAuthenticated)
            {
                //SendMail();
            }
        }
        
        public string SendMail()
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("denesice@yandex.ru", "Web Shop");
            // кому отправляем
            MailAddress to = new MailAddress(_database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().EMail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Покупка игры";
            // текст письма
            m.Body = "<div><br><h2 style='text-align: center'>Благодарим за покупку игры " + product.Name + ", ваш ключ: </h2><br><h1 style='text-align: center'>" + key + "</h1><div>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new System.Net.NetworkCredential("denesice@yandex.ru", "denesova");
            smtp.EnableSsl = true;
            smtp.Send(m);
            return "";
        }

        public string GetUserName()
        {
            return _database.User.Where(x => x.EMail == User.Identity.Name).FirstOrDefault().Name;
        }

    }
}