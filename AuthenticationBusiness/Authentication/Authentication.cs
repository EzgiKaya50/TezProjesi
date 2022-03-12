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

namespace AuthenticationBusiness.Authentication
{
    public class Authentication
    {
        private readonly IConfiguration _config;
        private readonly TezContext _context;

        public Authentication (TezContext context, IConfiguration config)
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

            //A claim is a statement about a subject by an issuer and
            //represent attributes of the subject that are useful in the context of authentication and authorization operations.
            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Actor, user.Name + " " + user.Surname)
                };

            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
            var principal = new ClaimsPrincipal(identity);
            //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return new LoginResponse("Giriş Başarılı.", true);
        }
    }
}
