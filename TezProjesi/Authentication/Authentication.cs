 using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration _config;
        private readonly TezContext _context;

        public Authentication(TezContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponse> Login_admin(HttpContext httpContext, LoginModel loginModel)
        {
            var user = _context.User
                .Where(x =>
                    x.Email == loginModel.Username &&
                    x.Password == loginModel.Password &&
                    x.UserType == 1 &&
                    x.Status == true)
                .FirstOrDefault();

            if (user == null)
            {
                return new LoginResponse("Kullanıcı adı veya şifreniz yanlış", false);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)), //kullanıcı bilgileri cookiye alınması için modellendi.
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Actor, user.Name + " " + user.Surname)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); //kullanıcı bilgileri cookiye alındı ve giriş yapıldı.Giriş işlemleri için mikrosoftun hazır kütüphaneleri kullanıldı

            return new LoginResponse("Giriş Başarılı.", true);
        }
        public async Task<LoginResponse> Login_user(HttpContext httpContext, LoginModel loginModel)
        {
            var user = _context.User
                .Where(x =>
                    x.Email == loginModel.Username &&
                    x.Password == loginModel.Password &&
                    x.UserType == 2 &&
                    x.Status == true)

                .FirstOrDefault();
            if (user == null)
            {
                return new LoginResponse("Kullanıcı adı veya şifreniz yanlış", false);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)), //kullanıcı bilgileri cookiye alınması için modellendi.
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Actor, user.Name + " " + user.Surname)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); //kullanıcı bilgileri cookiye alındı ve giriş yapıldı.Giriş işlemleri için mikrosoftun hazır kütüphaneleri kullanıldı

            return new LoginResponse("Giriş Başarılı.", true);
        }
    }
}

  

