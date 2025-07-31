using API.Data;
using API.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController(DataContext db) : ControllerBase {
    
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
      return Ok(await db.Users.ToListAsync());
    }
    
    
    [HttpGet("id{id}")] 
    public async Task<ActionResult<AppUser>> GetUser(uint id) {
      var user = await db.Users.FindAsync(id);
      if(user == null) return NotFound();
      return Ok(user);
    }
  }
}
