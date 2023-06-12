using CryptoProvider.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace CryptoProvider.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly RSA rsa;
        private static ConcurrentDictionary<Guid, string> refreshToAccessTokenMapStorage = new();

        private static string PublicKeyString { get; }
        private SecurityKey PublicKey
        {
            get
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(PublicKeyString), out _);
                return new RsaSecurityKey(rsa);
            }
        }

        private static string PrivateKeyString { get; }
        private SecurityKey PrivateKey
        {
            get
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(PrivateKeyString), out _);
                return new RsaSecurityKey(rsa);
            }
        }

        static AuthController()
        {
            rsa = RSA.Create();

            PrivateKeyString = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            PublicKeyString = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        }


        [HttpPost(nameof(LogIn))]
        public IActionResult LogIn()
        {
            var token = new JwtSecurityToken
            (
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddSeconds(15),
                signingCredentials: new SigningCredentials(PrivateKey, SecurityAlgorithms.RsaSha256)
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            string refreshToken = StoreAccessTokenAndGetRefreshToken(accessToken);

            return Ok(new LoginResponse() { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpGet(nameof(GetPublicKey))]
        public IActionResult GetPublicKey()
        {
            return Ok(PublicKeyString);
        }

        private string StoreAccessTokenAndGetRefreshToken(string accessToken)
        {
            Guid refreshToken = Guid.NewGuid();

            refreshToAccessTokenMapStorage.TryAdd(refreshToken, accessToken);

            return refreshToken.ToString();
        }
    }
}
