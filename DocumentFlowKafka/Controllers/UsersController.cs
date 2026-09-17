using DocumentFlowKafka.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentFlowKafka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private DocumentFlowContext _context;
        private readonly IConfiguration _config;

        UsersController(DocumentFlowContext _context, IConfiguration config)
        {
            this._context = _context;
            _config = config;
        }

        /// <summary>
        /// Сделаю авторизацию через отправку Uid пользователя, в ответ будет лететь JWT
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] string UId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var user = _context.Users.Single(s => s.Uid == UId);
            string userId = user.Id.ToString();
            string userName = user.NameOrg;

            // CLAIMS — данные, которые будут в токене
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return Ok(new 
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }
        
        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Users user)
        {
            user.Uid = Guid.NewGuid().ToString();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = user.Uid,
            });
        }
    }
}
