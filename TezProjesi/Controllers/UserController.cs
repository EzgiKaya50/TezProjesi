using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TezProjesi.CustomModels;
using TezProjesi.Models;
using static TezProjesi.CustomModels.HotelModels;

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
            model.CommentList = _context.Comments.Where(c => c.Status == true).Include(c => c.Hotel).Include(c => c.User).ToList();
            model.RoomList = _context.Room.Where(c => c.Status == true).ToList();
            model.RandomList = _context.Hotel.Where(c => c.Status == true && c.Image.Count > 0).Include(c => c.Image).OrderBy(c => Guid.NewGuid()).Take(4).ToList();
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
        public ActionResult Oteller()
        {
            List<Hotel> HotelList = _context.Hotel.Where(c => c.Status == true).Include(c => c.Image).ToList();

            return View(HotelList);
        }
        public ActionResult OtelDetay(int hotelId)
        {
            Hotel model = _context.Hotel.Where(c => c.Id == hotelId && c.Status == true).Include(x => x.Room).ThenInclude(c => c.RoomImages).FirstOrDefault();

            return View(model);
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
            model.Reservations = _context.Reservation.Where(c => c.Status == true && c.UserId == Convert.ToInt32(userID)).Include(c => c.Hotel).ThenInclude(c => c.Image).Include(c => c.Room).ToList();
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

                model.RoomList = _context.Room.Where(c => c.HotelId == model.reservation.HotelID && c.Adult == model.reservation.adults && c.Children == model.reservation.children && c.Status == true).Include(c => c.RoomImages).ToList();
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
        public ActionResult YorumKaydet(RandevuModels model)
        {

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            Comments comment = new Comments()
            {
                CreatedAt = DateTime.Now,
                Status = true,
                UpdatedAt = DateTime.Now,
                HotelId = model.hotelId,
                UserId = model.userId,
                UserComment = model.comment,
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Randevularım", "User");
        }

        public ActionResult RezervasyonDetay(int hotelId, int roomId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                var room = _context.Room.Where(c => c.HotelId == hotelId && c.Id == roomId && c.Status == true).Include(c => c.RoomImages).ToList();

                return View(room);
            }
            else
            {
                return RedirectToAction("UserLogin", "Giris");
            }
        }

        public async Task<ActionResult> ProfilGuncelleKaydet(ProfilModels model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            var existUser = await _context.User.FirstOrDefaultAsync(c => c.Id == model.User.Id);
            var existProfile = await _context.Profile.FirstOrDefaultAsync(c => c.Id == model.Profile.Id);

            existUser.Name = model.User.Name;
            existUser.Password = model.User.Password;
            existUser.Phone = model.User.Phone;
            existUser.Surname = model.User.Surname;
            existUser.UpdatedAt = DateTime.Now;

            _context.Entry(existUser).State = EntityState.Modified;

            if (existProfile == null)
            {
                Profile newProfile = new Profile()
                {
                    Address = model.Profile.Address,
                    CreatedAt = DateTime.Now,
                    Phone = model.User.Phone,
                    UpdatedAt = DateTime.Now,
                    UserId = existUser.Id
                };
                _context.Profile.Add(newProfile);

            }
            else
            {
                existProfile.Phone = model.User.Phone;
                existProfile.UpdatedAt = DateTime.Now;
                existProfile.Address = model.Profile.Address;
                _context.Entry(existProfile).State = EntityState.Modified;
            }

            _context.SaveChanges();
            return RedirectToAction("Profil", "User");
        }

        public ActionResult SaveReservation(Payment model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var user = _context.User.FirstOrDefault(c => c.Id == Convert.ToInt32(userID));
                var room = _context.Room.FirstOrDefault(c => c.Id == model.roomID);

                Reservation reservation = new Reservation()
                {
                    Adults = model.adult,
                    Checkin = false,
                    Checkout = false,
                    Children = model.children,
                    CreatedAt = DateTime.Now,
                    Email = user.Email,
                    Enddate = model.endDate,
                    Startdate = model.startDate,
                    Name = user.Name,
                    Days = (model.endDate - model.startDate).Days,
                    HotelId = model.hotelID,
                    Ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString(),
                    RoomId = model.roomID,
                    Note = null,
                    Surname = user.Surname,
                    Status = true,
                    UpdatedAt = DateTime.Now,
                    UserId = Convert.ToInt32(userID),
                    Phone = user.Phone,
                    Total = room.Price * (model.endDate - model.startDate).Days+1,
                };
                _context.Reservation.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("Randevularım","User");
            }
            else
            {
                return RedirectToAction("UserLogin","Giris");
            }
        }

        public IActionResult change_language(string returnUrl, string culture)
        {
            ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                SameSite = SameSiteMode.None,
                Secure = true,
                IsEssential = true
            });

            return LocalRedirect(returnUrl);
        }

        public ActionResult OdemeSayfasi(DateTime startDate, DateTime endDate, int children, int adults, int hotelID, int roomID)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var userID = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                Payment model = new Payment()
                {
                    startDate = startDate,
                    endDate = endDate,
                    children = children,
                    adult = adults,
                    hotelID = hotelID,
                    hotelName = _context.Hotel.FirstOrDefault(c => c.Id == hotelID).Title,
                    roomID = roomID,
                    roomName = _context.Room.FirstOrDefault(c => c.Id == roomID).Title,
                    totalPrice = (double)_context.Room.FirstOrDefault(c => c.Id == roomID).Price * ((endDate - startDate).Days+1),
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("UserLogin", "Giris");
            }
        }
    }
}
