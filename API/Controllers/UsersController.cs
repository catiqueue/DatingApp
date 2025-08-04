using API.Data;
using API.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(DataContext db) : ApiControllerBase {
    
  [HttpGet("all")]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
    return Ok(await db.Users.ToListAsync());
  }
    
  [HttpGet("{id}")] 
  public async Task<ActionResult<AppUser>> GetUser(uint id) {
    var user = await db.Users.FindAsync(id);
    if(user == null) return NotFound();
    return Ok(user);
  }
}
