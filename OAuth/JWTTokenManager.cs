using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiPOC.OAuth.Interface;

namespace WebApiPOC.OAuth
{
    public class JWTTokenManager : ITokenManager
    {
        //When the client sends a request with a JWT token in the Authorization header, the middleware automatically validates it.
        //To secure a controller or an action, use the[Authorize] attribute:


        readonly IConfiguration _config;
        public JWTTokenManager(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    //Ques - What will happen if JWT Token Expires during navigation?
    // Ans -
        //    Client Side:
                //The client attempts to make an API request.
                //If the JWT token is expired, the request receives a 401 Unauthorized response.
                //The client then sends a request to refresh the token.
                //If the refresh token is valid, the client receives a new JWT token and a new refresh token.
                //The client retries the original request with the new JWT token.
        //    Server Side:

                //The server provides an endpoint to refresh tokens.
                //This endpoint validates the refresh token and issues new JWT and refresh tokens if valid.

}
