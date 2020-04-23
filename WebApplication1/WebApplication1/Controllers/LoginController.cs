using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private IRefreshTokenService iref;
        private ILoginDBService ilog;
        private IConfiguration iconf;
        public LoginController(ILoginDBService service, IConfiguration configuration, IRefreshTokenService refr)
        {
            ilog = service;
            iconf = configuration;
            iref = refr;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            Student st = ilog.login(request, iconf["SQLEncryptKey"]);
            if (st == null)
            {
                return Unauthorized();
            }


            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,  st.IndexNumber),
                new Claim(ClaimTypes.Role, "employee"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconf["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var g = Guid.NewGuid().ToString();

            var token = new JwtSecurityToken
            (
                issuer: "cw7",
                audience: "students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            iref.addRefToken(request.login, g);

            return Ok(
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = g
                });
        }

        [HttpPost("refresh-token/{token}")]
        public IActionResult Refresh(string reftoken)
        {
            string check = iref.checkRefToken(reftoken);
            if (check == null)
                return Unauthorized();
            else 

            {
                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,  check),
                new Claim(ClaimTypes.Role, "employee"),
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconf["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var token = new JwtSecurityToken
                (
                    issuer: "cw7",
                    audience: "students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken = reftoken
                    });
            }
        }
    }
}
