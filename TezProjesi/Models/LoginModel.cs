using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TezProjesi.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public string Message { get; set; }
        public bool IsLoggedIn { get; set; }

        public LoginResponse(string message, bool isLoggedIn)
        {
            Message = message;
            IsLoggedIn = isLoggedIn;
        }
    }
}
