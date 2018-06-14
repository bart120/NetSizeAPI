using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetSizeAPI.Models;

namespace NetSizeAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private IConfiguration configuration;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<Object> Login([FromBody]AccountLoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Mail, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email == model.Mail);
                return await GenerateToken(user);
            }
            throw new ApplicationException("INVALID_LOGIN");
        }

        private async Task<object> GenerateToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expire,
                signingCredentials: cred);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        public async Task<object> Register([FromBody]AccountLoginViewModel model)
        {
            var u = new IdentityUser
            {
                UserName = model.Mail,
                Email = model.Mail
            };
            var result = await userManager.CreateAsync(u, model.Password);
            if (result.Succeeded)
                return "OK";
            throw new ApplicationException("INVALID_USER");
        }
    }
}