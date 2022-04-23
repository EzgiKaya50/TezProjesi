using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            model.reservation = new ReservationCRUD { startdate = DateTime.Now, enddate = DateTime.Now };

            return View(model);
        }
        public IActionResult Kaydol(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        public IActionResult KayitOlustur(User model)
        {
            List<string> existMails = _context.User.Where(c => c.Status == true && c.UserType == 2).Select(c => c.Email).ToList();
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
            List<Hotel> HotelList = _context.Hotel.Where(c => c.Status == true).Include(c => c.Image).ToList();

            return View(HotelList);
        }
        public ActionResult OtelDetay(int hotelId)
        {
            Hotel model = _context.Hotel.Where(c => c.Id==hotelId && c.Status == true).Include(x => x.Room).ThenInclude(c => c.RoomImages).FirstOrDefault();

          return  View(model);
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
                Profile = _context.Profile.FirstOrDefault(c => c.UserId == Convert.ToInt32(userID)),
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
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            RandevuModels model = new RandevuModels();
            model.Reservations = _context.Reservation.Where(c => c.Status == true && c.UserId == Convert.ToInt32(userID)).ToList();
            return View(model);
        }
        public ActionResult RezervasyonYap(DateTime startDate, DateTime endDate, int children, int adults, int hotelID, int roomID)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var user = _context.User.FirstOrDefault(c => c.Id == Convert.ToInt32(userID));
            var room = _context.Room.FirstOrDefault(c => c.Id == roomID);

            Reservation reservation = new Reservation()
            {
                Adults = adults,
                Checkin = false,
                Checkout = false,
                Children = children,
                CreatedAt = DateTime.Now,
                Email = user.Email,
                Enddate = endDate,
                Startdate = startDate,
                Name = user.Name,
                Days = (endDate - startDate).Days,
                HotelId = hotelID,
                Ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString(),
                RoomId = roomID,
                Note = null,
                Surname = user.Surname,
                Status = true,
                UpdatedAt = DateTime.Now,
                UserId = Convert.ToInt32(userID),
                Phone = user.Phone,
                Total = room.Price,
            };
            _context.Reservation.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Randevularım", "User");
        }
        public ActionResult RezervasyonAra(HomePageModels model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                model.RoomList = _context.Room.Where(c => c.HotelId == model.reservation.HotelID && c.Adult == model.reservation.adults && c.Children == model.reservation.children && c.Status == true).ToList();
                var existReservation = _context.Reservation.Where(c => c.HotelId == model.reservation.HotelID && (c.Startdate >= model.reservation.startdate && c.Enddate <= model.reservation.startdate)).ToList();
                foreach (var room in existReservation)
                {
                    if (model.RoomList.Select(c => c.Id).Contains((int)room.RoomId))
                    {
                        model.RoomList.Remove(model.RoomList.FirstOrDefault(c => c.Id == room.RoomId));
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("UserLogin", "Giris");
            }
        }
    }
}
