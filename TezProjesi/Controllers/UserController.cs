using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TezProjesi.CustomModels;
using TezProjesi.Models;

namespace TezProjesi.Controllers
{
    public class UserController : Controller
    {
        private readonly TezContext _context;

        public UserController(TezContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomePageModels model = new HomePageModels();
            model.FaqList = _context.Faq.Where(c => c.Status == true).ToList();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            model.CommentList = _context.Comment.Where(c => c.Status == true).ToList();
            model.RoomList = _context.Room.Where(c => c.Status == true).ToList();

            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
               
            }
            model.reservation = new ReservationCRUD {startdate=DateTime.Now,enddate=DateTime.Now };

               return View(model);
        }
        public IActionResult Kaydol(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        public IActionResult KayitOlustur(User model)
        {
            List<string> existMails = _context.User.Where(c => c.Status == true && c.UserType==2).Select(c => c.Email).ToList();
            if (existMails.Contains(model.Email))
            {

                return RedirectToAction("Kaydol", "User", new { message = "E-mail sistemde kayıtlı." });
            }
            else
            {
                model.CreatedAt = DateTime.Now;
                model.Status = true;
                model.UpdatedAt = DateTime.Now;
                model.UserType = 2;
                _context.User.Add(model);
                _context.SaveChanges();
                return RedirectToAction("LoginUser", "Giris");
            }     
        }
        //public ActionResult Hakkımızda()
        //{

        //    return View();
        //}
        public ActionResult İletisim()
        {
            return View();
        }
        public ActionResult Oteller()
        {
            return View();
        }
        public async Task<IActionResult> CikisYapUser()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "User");
        }
        [Authorize(Roles = "User")]
        public ActionResult Profil()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            ProfilModels model = new ProfilModels
            {
                User = _context.User.FirstOrDefault(c => c.Id == Convert.ToInt32(userID) && c.Status == true),
                Profile=_context.Profile.FirstOrDefault(c=> c.UserId== Convert.ToInt32(userID)),
            };

            if (model.Profile == null)
            {
                model.Profile = new Profile();
            }

            if (model.Profile.Address == null)
            {
                model.Profile.Address = "";
            }
            if (model.Profile.Phone == null)
            {
                model.Profile.Phone = "";
            }

            return View(model);
        }
        public ActionResult Randevularım()
        {
            return View();
        }
        public ActionResult RezervasyonAra(HomePageModels model)
        {
            return View();
        }
    }
}
