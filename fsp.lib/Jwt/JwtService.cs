using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using fsp.lib.Appsettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace fsp.lib.Jwt;
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateToken(string UserId)
    {
        var lowerCase_UserId = UserId.ToLower();
        //generate token
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var jwtTokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Key!);
        var jwtTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(
                new Claim[]{new Claim(ClaimTypes.Name, lowerCase_UserId) }
            ),            
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiredInMin),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtTokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = jwtTokenHandler.CreateToken(jwtTokenDescriptor);
        return jwtTokenHandler.WriteToken(securityToken);
    }
 
}
