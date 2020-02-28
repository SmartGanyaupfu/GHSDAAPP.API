using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GHSDAAPP.Data;
using GHSDAAPP.Dtos;
using GHSDAAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GHSDAAPP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            this.config = config;
        }

        [HttpPost("register")]

        // this wil use a DTO since it wil recieve data as a serialized json object
        public async Task<IActionResult> Register(UserForRegisterDto userRegDto)
        {
            //validate request

            //nomalize for consistent data in db

            userRegDto.Username = userRegDto.Username.ToLower();
            if (await _repo.UserExists(userRegDto.Username))
                return BadRequest("Username already exits");
            var userToCreate = new User
            {
                Name=userRegDto.Name,
                Username = userRegDto.Username
                
            };

            var createdUser = await _repo.Register(userToCreate, userRegDto.Password );
            return StatusCode(201);


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(this.config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //return as an object to our client
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}