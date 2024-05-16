using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyPMO_.NET.Services
{
    public class JwtService : IJwt
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string JwtTokenGenerator(Employee employee)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                new(ClaimTypes.Name, employee.Username),
                new(ClaimTypes.Role, employee.Tier.Name)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.GetValue<string>("JwtSettings:Issuer"),
                Audience = _config.GetValue<string>("JwtSettings:Audience"),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSettings:Key"))), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
