using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace TestJWKSServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {
        /// <summary>
        /// List to store RSA key pairs
        /// </summary>
        public readonly List<RSAParameters> _keys;

        /// <summary>
        /// Constructor to initialize the list of RSA key pairs
        /// </summary>
        public JwtController()
        {
            _keys = new List<RSAParameters>();
            // Generate RSA key pair and add to the list
            using RSA rsa = RSA.Create();
            _keys.Add(rsa.ExportParameters(true));
        }

        /// <summary>
        /// Endpoint to get JWKS (JSON Web Key Set)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJwks")]
        public IActionResult GetJwks()
        {
            var keys = new List<object>();
            foreach (var key in _keys)
            {
                if (!IsExpired(key))
                {
                    // Construct JWKS entry for each key
                    keys.Add(new
                    {
                        kid = Guid.NewGuid().ToString(), // Unique Key ID
                        kty = "RSA",
                        alg = "RS256",
                        use = "sig",
                        e = Base64UrlEncoder.Encode(key.Exponent),
                        n = Base64UrlEncoder.Encode(key.Modulus)
                    });
                }
            }
            var jwks = new { keys };
            return Ok(JsonSerializer.Serialize(jwks));
        }

        /// <summary>
        /// Endpoint to authenticate and generate JWT
        /// </summary>
        /// <param name="expired"></param>
        /// <returns></returns>
        [HttpPost("auth")]
        public IActionResult Authenticate(bool expired = false)
        {
            // Use the first key that is not expired
            var key = _keys.FirstOrDefault(k => !IsExpired(k));
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = new RsaSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Samixa Rajop"),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(creds, SecurityAlgorithms.RsaSha256)
            };

            if (expired)
            {
                // Adjust token expiry for testing expired JWT
                tokenDescriptor.Expires = DateTime.UtcNow;
                tokenDescriptor.NotBefore = DateTime.UtcNow.AddMinutes(-30);
                creds = new RsaSecurityKey(_keys[0]);
                tokenDescriptor.SigningCredentials = new SigningCredentials(creds, SecurityAlgorithms.RsaSha256);
            }

            // Create JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new JwtToken()
            {
                Token = tokenString
            });
        }

        /// <summary>
        /// Helper method to check if RSA key is expired
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool IsExpired(RSAParameters key)
        {
            // Assume key expires in one hour
            var expiry = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            return expiry <= DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStatus")]
        public IActionResult GetStatus()
        {
            return Ok();
        }
    }
}
