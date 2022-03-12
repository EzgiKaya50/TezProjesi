using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TezProjesi.Authentication;
using TezProjesi.Models;

namespace TezProjesi.Controllers
{
    public class GirisController : Controller
    {
        private readonly TezContext _context;
        private readonly IAuthentication _authentication;

        public GirisController(TezContext context, IAuthentication authentication)
        {
            _context = context;
            _authentication = authentication;
        }


        public ActionResult LoginAdmin(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public async Task<ActionResult> AdminLogin(LoginModel model)
        {
            LoginResponse loginResponse = await _authentication.Login_admin(HttpContext, model);

            if (loginResponse.IsLoggedIn)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("LoginAdmin", "Giris", new { message = loginResponse.Message });
            }
        }
        public ActionResult LoginUser(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        public async Task<ActionResult> UserLogin(LoginModel model)
        {
            LoginResponse loginResponse = await _authentication.Login_user(HttpContext, model);

            if (loginResponse.IsLoggedIn)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("LoginUser", "Giris", new { message = loginResponse.Message });
            }
        }
   
    }
}
