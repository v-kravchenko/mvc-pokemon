using Pokemon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pokemon.Controllers
{
    public class HomeController : Controller
    {
        PokemonContext db = new PokemonContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            //Получаем список заказов по каждому пользователю
            var rows = from u in db.Users
                       join o in db.Orders on u.Id equals o.UserId into os
                       from o in os.DefaultIfEmpty()
                       select new
                       {
                           UserId = u.Id,
                           u.Name,
                           DateAdd = o == null ? u.DateAdd : o.DateAdd
                       } into rr
                       group rr by rr.UserId into g
                       select new
                       {
                           UserId = g.Key,
                           Name = g.Select(a => a.Name).FirstOrDefault(),
                           Count = g.Count(),
                           DateAdd = g.Max(x => x.DateAdd)
                       };

            //Формируем ленту  
            var list = new List<string>();

            foreach (var item in rows)
                list.Add(
                    String.Format("{0} {1} заказал покемона уже {2} раз", 
                    item.DateAdd.ToString("HH:mm dd.MM.yyyy"), 
                    item.Name, 
                    item.Count)
                );

            return View(list);
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(User user)
        {
            //Проверяем существует ли пользователь по задаyному email
            User dbUser = db.Users.FirstOrDefault(x => x.Email == user.Email);

            if (dbUser == null)
            {
                //Создаём нового пользователя
                dbUser = db.Users.Add(new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    DateAdd = DateTime.Now
                });
            }
            
            //Создаем новый заказ
            var neworder = db.Orders.Add(new Order()
            {
                User = dbUser,
                DateAdd = DateTime.Now
            });

            db.SaveChanges();

            return RedirectToAction("List");
        }

    }
}