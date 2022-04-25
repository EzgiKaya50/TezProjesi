using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using TezProjesi.Models;
using static TezProjesi.CustomModels.CategoryModels;
using static TezProjesi.CustomModels.CommentModels;
using static TezProjesi.CustomModels.FaqModels;
using static TezProjesi.CustomModels.HotelModels;
using static TezProjesi.CustomModels.MessageModels;
using static TezProjesi.CustomModels.SettingModels;
using TezProjesi.CustomModels;
using static TezProjesi.CustomModels.RoomModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using static TezProjesi.CustomModels.GaleriModels;
using static TezProjesi.CustomModels.RoomGaleriModels;

namespace TezProjesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TezContext _context;

        public AdminController(TezContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            DashboardModels model = new DashboardModels();
            model.CategoryCount = _context.Category.Where(c => c.Status == true).Count();
            model.HotelCount = _context.Hotel.Where(c => c.Status == true).Count();
            model.CommentCount = _context.Comments.Where(c => c.Status == true).Count();
            model.MessageCount = _context.Message.Where(c => c.Status == true).Count();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Calendar()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            CalendarModel model = new CalendarModel()
            {
                checkinList = _context.Reservation.Where(c => c.Status == true && c.Checkin == false && c.Checkout == false).Include(c => c.Hotel).Include(c => c.User).Include(c => c.Room).ToList(),
                checkoutList = _context.Reservation.Where(c => c.Status == true && c.Checkin == true && c.Checkout == false).Include(c => c.Hotel).Include(c => c.User).Include(c => c.Room).ToList(),
                finishedList = _context.Reservation.Where(c => c.Status == true && c.Checkin == true && c.Checkout == true).Include(c => c.Hotel).Include(c => c.User).Include(c => c.Room).ToList(),
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Categories()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            YeniKategoriEklePage model = new YeniKategoriEklePage();
            model.CategoryList = _context.Category.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Hotels()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            HotelPage model = new HotelPage();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Comments()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            CommentPage model = new CommentPage();
            model.CommentList = _context.Comments.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult FAQ()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            FaqPage model = new FaqPage();
            model.FaqList = _context.Faq.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SoruEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            FaqPage model = new FaqPage();
            model.FaqCRUD = new Faq();
            model.FaqList = _context.Faq.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SoruEkleKaydet(FaqPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            model.FaqCRUD.CreatedAt = DateTime.Now;
            model.FaqCRUD.Status = true;
            model.FaqCRUD.UpdatedAt = DateTime.Now;

            _context.Faq.Add(model.FaqCRUD);
            _context.SaveChanges();
            return RedirectToAction("FAQ", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SoruGuncelle(int faqId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            FaqPage model = new FaqPage();
            model.FaqCRUD = _context.Faq.FirstOrDefault(c => c.Id == faqId);
            return View(model);

        }
        [Authorize(Roles = "Admin")]
        public ActionResult SoruGuncelleKaydet(FaqPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var existFaq = _context.Faq.FirstOrDefault(c => c.Id == model.FaqCRUD.Id);
            existFaq.Question = model.FaqCRUD.Question;
            existFaq.Answer = model.FaqCRUD.Answer;
            existFaq.UpdatedAt = DateTime.Now;
            _context.Entry(existFaq).State = EntityState.Modified;
            _context.SaveChanges();



            return RedirectToAction("FAQ", "Admin");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult SoruSil(int faqId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existFaq = _context.Faq.FirstOrDefault(c => c.Id == faqId);
            existFaq.Status = false;
            existFaq.UpdatedAt = DateTime.Now;
            _context.Entry(existFaq).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("FAQ", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult YeniAyarEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            SettingPage model = new SettingPage();
            model.SettingCRUD = new Setting();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AyarGuncelle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            SettingPage model = new SettingPage();
            model.SettingCRUD = _context.Setting.FirstOrDefault();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AyarGuncelleKaydet(SettingPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var existSetting = _context.Setting.FirstOrDefault(c => c.Id == model.SettingCRUD.Id);
            existSetting.Company = model.SettingCRUD.Company;
            existSetting.Title = model.SettingCRUD.Title;
            existSetting.Keywords = model.SettingCRUD.Keywords;
            existSetting.Description = model.SettingCRUD.Description;
            existSetting.Adress = model.SettingCRUD.Adress;
            existSetting.Fax = model.SettingCRUD.Fax;
            existSetting.Phone = model.SettingCRUD.Phone;
            existSetting.Instagram = model.SettingCRUD.Instagram;
            existSetting.Aboutus = model.SettingCRUD.Aboutus;
            existSetting.Contact = model.SettingCRUD.Contact;
            existSetting.Email = model.SettingCRUD.Email;
            existSetting.Facebook = model.SettingCRUD.Facebook;
            existSetting.Referencs = model.SettingCRUD.Referencs;
            existSetting.Smtpemail = model.SettingCRUD.Smtpemail;
            existSetting.Smtppassword = model.SettingCRUD.Smtppassword;
            existSetting.Smtpport = model.SettingCRUD.Smtpport;
            existSetting.Smtpserver = model.SettingCRUD.Smtpserver;
            existSetting.Twitter = model.SettingCRUD.Twitter;
            existSetting.UpdatedAt = DateTime.Now;
            _context.Entry(existSetting).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("AyarGuncelle", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AyarSil(int settingId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existSetting = _context.Setting.FirstOrDefault(c => c.Id == settingId);
            existSetting.Status = false;
            existSetting.UpdatedAt = DateTime.Now;
            _context.Entry(existSetting).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Setting", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Messages()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            MessagePage model = new MessagePage();
            model.MessageList = _context.Message.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult YeniKategoriEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            YeniKategoriEklePage model = new YeniKategoriEklePage();
            model.CategoryCRUD = new Category();
            model.CategoryList = _context.Category.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KategoriGuncelle(int categoryId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            YeniKategoriEklePage model = new YeniKategoriEklePage();
            model.CategoryCRUD = _context.Category.FirstOrDefault(c => c.Id == categoryId);
            model.CategoryList = _context.Category.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KategoriGuncelleKaydet(YeniKategoriEklePage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            #region file op.
            if (model.CategoryImage != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.CategoryImage.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.CategoryImage.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.CategoryImage.CopyTo(stream);
                }
                model.CategoryCRUD.Image = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion


            var existCategory = _context.Category.FirstOrDefault(c => c.Id == model.CategoryCRUD.Id);
            existCategory.Image = model.CategoryCRUD.Image;
            existCategory.Description = model.CategoryCRUD.Description;
            existCategory.Keywords = model.CategoryCRUD.Keywords;
            existCategory.Parentid = model.CategoryCRUD.Parentid;
            existCategory.Title = model.CategoryCRUD.Title;
            existCategory.UpdatedAt = DateTime.Now;
            _context.Entry(existCategory).State = EntityState.Modified;
            _context.SaveChanges();



            return RedirectToAction("Categories", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KategoriSil(int categoryId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existCategory = _context.Category.FirstOrDefault(c => c.Id == categoryId);
            existCategory.Status = false;
            existCategory.UpdatedAt = DateTime.Now;
            _context.Entry(existCategory).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Categories", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult YeniOtelEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            HotelPage model = new HotelPage();
            model.HotelCRUD = new Hotel();
            model.CategoryList = _context.Category.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OtelGuncelle(int hotelId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            HotelPage model = new HotelPage();
            model.HotelCRUD = _context.Hotel.FirstOrDefault(c => c.Id == hotelId);
            model.CategoryList = _context.Category.Where(c => c.Status == true).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OtelGuncelleKaydet(HotelPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var existHotels = _context.Hotel.FirstOrDefault(c => c.Id == model.HotelCRUD.Id);
            existHotels.Address = model.HotelCRUD.Address;
            existHotels.CategoryId = model.HotelCRUD.CategoryId;
            existHotels.City = model.HotelCRUD.City;
            existHotels.Country = model.HotelCRUD.Country;
            existHotels.Description = model.HotelCRUD.Description;
            existHotels.Detail = model.HotelCRUD.Detail;
            existHotels.Email = model.HotelCRUD.Email;
            existHotels.Fax = model.HotelCRUD.Fax;
            existHotels.Keywords = model.HotelCRUD.Keywords;
            existHotels.Location = model.HotelCRUD.Location;
            existHotels.Phone = model.HotelCRUD.Phone;
            existHotels.Star = model.HotelCRUD.Star;
            existHotels.Title = model.HotelCRUD.Title;
            existHotels.UpdatedAt = DateTime.Now;
            existHotels.Userid = Convert.ToInt32(userId);
            _context.Entry(existHotels).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Hotels", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OtelSil(int hotelId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existHotels = _context.Hotel.FirstOrDefault(c => c.Id == hotelId);
            existHotels.Status = false;
            existHotels.UpdatedAt = DateTime.Now;
            _context.Entry(existHotels).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Hotels", "Admin");
        }
        [Authorize(Roles = "Admin")]

        public ActionResult Rooms()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomPage model = new RoomPage();
            model.RoomList = _context.Room.Where(c => c.Status == true).ToList();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            model.RoomType = new RoomType();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        public ActionResult OdaEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomPage model = new RoomPage();
            model.RoomCRUD = new Room();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            model.RoomType = new RoomType();
            return View(model);

        }
        [Authorize(Roles = "Admin")]

        public ActionResult OdaKaydet(RoomPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            model.RoomCRUD.Status = true;
            model.RoomCRUD.CreatedInt = DateTime.Now;
            model.RoomCRUD.UpdatedAt = DateTime.Now;
            _context.Room.Add(model.RoomCRUD);
            _context.SaveChanges();
            return RedirectToAction("Rooms", "Admin");

        }
        [Authorize(Roles = "Admin")]

        public ActionResult OdaGuncelle(int roomId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomPage model = new RoomPage();
            model.RoomCRUD = _context.Room.FirstOrDefault(c => c.Id == roomId);
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            model.RoomType = new RoomType();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        public ActionResult OdaGuncelleKaydet(RoomPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var existRooms = _context.Room.FirstOrDefault(c => c.Id == model.RoomCRUD.Id);
            existRooms.Adet = model.RoomCRUD.Adet;
            existRooms.HotelId = model.RoomCRUD.HotelId;
            existRooms.Price = model.RoomCRUD.Price;
            existRooms.Title = model.RoomCRUD.Title;
            existRooms.Type = model.RoomCRUD.Type;
            existRooms.Description = model.RoomCRUD.Description;
            existRooms.UpdatedAt = DateTime.Now;
            _context.Entry(existRooms).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Rooms", "Admin");
        }
        [Authorize(Roles = "Admin")]

        public ActionResult OdaSil(int roomId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existRooms = _context.Room.FirstOrDefault(c => c.Id == roomId);
            existRooms.Status = false;
            existRooms.UpdatedAt = DateTime.Now;
            _context.Entry(existRooms).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Rooms", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Galeri()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            GaleriPage model = new GaleriPage();
            model.ImageList = _context.Image.ToList();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            return View(model);
        }
        public ActionResult FotografEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            GaleriPage model = new GaleriPage();
            model.ImageCRUD = new Image();
            model.HotelList = _context.Hotel.Where(c => c.Status == true).ToList();
            return View(model);
        }
        public ActionResult FotografGuncelle(int GaleriId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            GaleriPage model = new GaleriPage();
            model.ImageCRUD = _context.Image.FirstOrDefault(c => c.Id == GaleriId);
            model.HotelList = _context.Hotel.ToList();
            return View(model);
        }
        public ActionResult FotografGuncelleKaydet(GaleriPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            #region file op.
            if (model.Image != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.Image.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.Image.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HotelImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                model.ImageCRUD.HotelImg = fileName;
            }
            else
            {
                //no  need op.
            }
            #endregion
            var existImage = _context.Image.FirstOrDefault(c => c.Id == model.ImageCRUD.Id);
            existImage.HotelId = model.ImageCRUD.HotelId;
            existImage.Title = model.ImageCRUD.Title;
            if (model.Image != null)
            {
                existImage.HotelImg = model.ImageCRUD.HotelImg;
            }
            _context.Entry(existImage).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Galeri", "Admin");
        }
        public ActionResult FotografKaydet(GaleriPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            #region file op.
            if (model.Image != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.Image.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.Image.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HotelImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                model.ImageCRUD.HotelImg = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion
            _context.Image.Add(model.ImageCRUD);
            _context.SaveChanges();
            return RedirectToAction("Galeri", "Admin");


        }
        public ActionResult FotografSil(int GaleriId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existImage = _context.Image.FirstOrDefault(c => c.Id == GaleriId);
            _context.Image.Remove(existImage);
            _context.SaveChanges();
            return RedirectToAction("Galeri", "Admin");
        }
        public ActionResult RoomGaleri()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomGaleriPage model = new RoomGaleriPage();
            model.ImageList = _context.RoomImages.Include(c => c.Room).ThenInclude(c => c.Hotel).ToList();
            model.RoomList = _context.Room.Include(c => c.Hotel).Where(c => c.Status == true).ToList();
            return View(model);
        }
        public ActionResult OdaFotografEkle()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomGaleriPage model = new RoomGaleriPage();
            model.RoomImgCRUD = new RoomImages();
            model.RoomList = _context.Room.Where(c => c.Status == true).Include(c => c.Hotel).ToList();

            foreach (var room in model.RoomList)
            {
                room.Title = room.Title + "/" + room.Hotel.Title;
            }
            return View(model);

        }
        public ActionResult OdaFotografGuncelle(int RoomGaleriId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            RoomGaleriPage model = new RoomGaleriPage();
            model.RoomImgCRUD = _context.RoomImages.FirstOrDefault(c => c.Id == RoomGaleriId);
            model.RoomList = _context.Room.Where(c => c.Status == true).Include(c => c.Hotel).ToList();

            foreach (var room in model.RoomList)
            {
                room.Title = room.Title + "/" + room.Hotel.Title;
            }
            return View(model);

        }
        public ActionResult OdaFotografKaydet(RoomGaleriPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            #region file op.
            if (model.RoomImg != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.RoomImg.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.RoomImg.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RoomImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.RoomImg.CopyTo(stream);
                }
                model.RoomImgCRUD.RoomImg = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion
            _context.RoomImages.Add(model.RoomImgCRUD);
            _context.SaveChanges();
            return RedirectToAction("RoomGaleri", "Admin");


        }
        public ActionResult OdaFotografGuncelleKaydet(RoomGaleriPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            #region file op.
            if (model.RoomImg != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.RoomImg.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.RoomImg.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RoomImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.RoomImg.CopyTo(stream);
                }
                model.RoomImgCRUD.RoomImg = fileName;
            }
            else
            {
                //no  need op.
            }
            #endregion
            var existRoomImg = _context.RoomImages.FirstOrDefault(c => c.Id == model.RoomImgCRUD.Id);
            existRoomImg.RoomId = model.RoomImgCRUD.RoomId;
            existRoomImg.Title = model.RoomImgCRUD.Title;
            if (model.RoomImg != null)
            {
                existRoomImg.RoomImg = model.RoomImgCRUD.RoomImg;
            }
            _context.Entry(existRoomImg).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("RoomGaleri", "Admin");
        }
        public ActionResult OdaFotografSil(int RoomGaleriId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existRoomImg = _context.RoomImages.FirstOrDefault(c => c.Id == RoomGaleriId);
            _context.RoomImages.Remove(existRoomImg);
            _context.SaveChanges();
            return RedirectToAction("RoomGaleri", "Admin");
        }

        public ActionResult Checkin(int reservationId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existreservation = _context.Reservation.FirstOrDefault(c => c.Id == reservationId);
            existreservation.Checkin = true;
            existreservation.UpdatedAt = DateTime.Now;
            _context.Entry(existreservation).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Calendar", "Admin");
        }

        public ActionResult Checkout(int reservationId)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var existreservation = _context.Reservation.FirstOrDefault(c => c.Id == reservationId);
            existreservation.Checkout = true;
            existreservation.UpdatedAt = DateTime.Now;
            _context.Entry(existreservation).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Calendar", "Admin");
        }

        public ActionResult KategoriKaydet(YeniKategoriEklePage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            #region file op.
            if (model.CategoryImage != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.CategoryImage.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.CategoryImage.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.CategoryImage.CopyTo(stream);
                }
                model.CategoryCRUD.Image = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion
            model.CategoryCRUD.Status = true;
            model.CategoryCRUD.CreatedAt = DateTime.Now;
            model.CategoryCRUD.UpdatedAt = DateTime.Now;
            _context.Category.Add(model.CategoryCRUD);
            _context.SaveChanges();
            return RedirectToAction("Categories", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OtelKaydet(HotelPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            model.HotelCRUD.Status = true;
            model.HotelCRUD.CreatedAt = DateTime.Now;
            model.HotelCRUD.UpdatedAt = DateTime.Now;
            _context.Hotel.Add(model.HotelCRUD);
            _context.SaveChanges();
            return RedirectToAction("Hotels", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AyarKaydet(SettingPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            model.SettingCRUD.Status = true;
            model.SettingCRUD.CreatedAt = DateTime.Now;
            model.SettingCRUD.UpdatedAt = DateTime.Now;
            _context.Setting.Add(model.SettingCRUD);
            _context.SaveChanges();
            return RedirectToAction("Setting", "Admin");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CikisYapAdmin()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginAdmin", "Giris");
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
