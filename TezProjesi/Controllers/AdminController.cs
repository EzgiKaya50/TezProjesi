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

namespace TezProjesi.Controllers
{
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
            model.CommentCount = _context.Comment.Where(c => c.Status == true).Count();
            model.MessageCount = _context.Message.Where(c => c.Status == true).Count();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Calendar()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            return View();
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
            model.CommentList = _context.Comment.Where(c => c.Status == true).ToList();
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
            model.FaqCRUD = _context.Faq.FirstOrDefault(c=>c.Id==faqId);
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



            return RedirectToAction("Categories","Admin");
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

            #region file op.
            if (model.HotelImage != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.HotelImage.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.HotelImage.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HotelImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.HotelImage.CopyTo(stream);
                }
                model.HotelCRUD.Image = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion
            var existHotels = _context.Hotel.FirstOrDefault(c => c.Id == model.HotelCRUD.Id);
            existHotels.Address = model.HotelCRUD.Address;
            existHotels.CategoryId = model.HotelCRUD.CategoryId;
            existHotels.City = model.HotelCRUD.City;
            existHotels.Country = model.HotelCRUD.Country;
            existHotels.Description = model.HotelCRUD.Description;
            existHotels.Detail = model.HotelCRUD.Detail;
            existHotels.Email = model.HotelCRUD.Email;
            existHotels.Fax = model.HotelCRUD.Fax;
            existHotels.Image = model.HotelCRUD.Image;
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
        [HttpPost]
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
            return RedirectToAction("Categories","Admin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OtelKaydet(HotelPage model)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var userId = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            #region file op.
            if (model.HotelImage != null)
            {
                string filePath = "";
                string extension = Path.GetExtension(model.HotelImage.FileName);
                Random rnd = new Random();
                int rndx = rnd.Next(100, 999);
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.HotelImage.FileName);
                string fileName = "UsrID-" + userId + "" + DateTime.Now.ToShortDateString().Replace(".", "-") + "" + FileNameWithoutExtension + "_" + rndx + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HotelImg", fileName);
                filePath = path;
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    model.HotelImage.CopyTo(stream);
                }
                model.HotelCRUD.Image = fileName;
            }
            else
            {
                //no  need op.
            }

            #endregion
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

        public ActionResult Error()
        {
            return View();
        }
    }
}
