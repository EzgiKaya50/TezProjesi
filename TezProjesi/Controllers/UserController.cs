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
            if(HttpContext.User.Identity.IsAuthenticated == true)
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            }
               return View();
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
    }
}
