using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProvider.Auth.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
