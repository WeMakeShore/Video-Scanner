using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFilesChecker
{
    public class LoginModel
    {
        public LoginModelProps Login { get; set; }
        public LoginModel(LoginModelProps login)
        {
            this.Login = login;
        }
    }

    public class LoginModelProps
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public LoginModelProps(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}