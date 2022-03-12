using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezProjesi.Models;

namespace TezProjesi.Authentication
{
    public interface IAuthentication
    {
        Task<LoginResponse> Login_admin(HttpContext httpContext, LoginModel loginModel);
        Task<LoginResponse> Login_user(HttpContext httpContext, LoginModel loginModel);


    }
}
