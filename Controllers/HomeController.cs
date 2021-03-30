using Kutse_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Kutse_App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Ootan sind minu peole! Palun tule!!!";
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 10 ? "Tere hommikust!" : "Tere päevast!";
            return View();
        }
        [HttpGet]
        public ViewResult Ankeet()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {   
            
            E_mail(guest); // функция для отправки письма с ответом
            ViewBag.Mail = guest.Email;
            if (ModelState.IsValid && (bool)guest.WillAttend)
            {
                db.Guests.Add(guest);
                db.SaveChanges();
                ViewBag.Message = "Tahad saada meeldetuletus meilile?" + guest.Email;
            }
            else
            {
                ViewBag.Message = "Sinu vastus on tähtis meie jaoks!";    
            }
            
             return View("Thanks", guest);
        }
        public void E_mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "programmeeriminemvc@gmail.com";//
                WebMail.Password = "************";//
                WebMail.From = "programmeeriminemvc@gmail.com";
                WebMail.Send("marina.oleinik@tthk.ee", "Vastus kutsele",guest.Name + " vastas " + ((guest.WillAttend ?? false) ? "tuleb peole " : "ei tule peole"));              
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju!Ei saa kirja saada!!!";
            }
        }
        public void Email_to_guest(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "programmeeriminemvc@gmail.com";//
                WebMail.Password = "************";//
                WebMail.From = "programmeeriminemvc@gmail.com";
                WebMail.Send(guest.Email, "Ära unusta!", "Sina lubas peole tulla " );
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju!Ei saa kirja saada!!!";
            }

        }
        //[HttpPost]
        public ViewResult Meeldetuletus(Guest guest)
        {
            if (ModelState.IsValid)
            {
                Email_to_guest(guest);
                ViewBag.Message = "Meeldetuleus oli saatnud meilile" + guest.Email;
            }
            else
            {
                ViewBag.Message = "Viga meiliga" + guest.Email;
            }
            return View("Index");
        }
        
    GuestContext db = new GuestContext();
        [Authorize] //- Данное представление Guests сможет увидить только авторизированный пользователь
        public ActionResult Guests()
        {
            IEnumerable<Guest> guests = db.Guests;
            
            return View(guests);
        }

    [HttpGet]
    public ActionResult Accept() //List<Guest> guests_true
        {
            IEnumerable<Guest> guests = db.Guests.Where(g => g.WillAttend == true);
            return View(guests);
    }

    [HttpGet]
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        public ActionResult Create(Guest guest)
        {
            db.Guests.Add(guest);
            db.SaveChanges();
            return RedirectToAction("Guests");

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Guest g = db.Guests.Find(id);
            if (g==null)
            {
                return HttpNotFound();
            }
            return View(g);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest g = db.Guests.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            db.Guests.Remove(g);
            db.SaveChanges();
            return RedirectToAction("Guests");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Guest g = db.Guests.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            return View(g);
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult EditConfirmed(Guest guest)
        {
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Guests");
        }
    }
}