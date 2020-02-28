using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GHSDAAPP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GHSDAAPP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        //Get api/values
        /*[HttpGet]
        public IActionResult GetMembers()
        {
            var memberValues = _context.Users.ToList();
            return Ok(memberValues);
        }*/
        /* Async call*/

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var memberValues = await _context.Users.ToListAsync();
            return Ok(memberValues);
        }
        [AllowAnonymous]
        //Get api/values/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var memberValue = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(memberValue);
        }
    }
}