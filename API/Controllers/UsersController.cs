using System.IdentityModel.Tokens.Jwt;

using API.Data.DTOs;
using API.Data.Repositories;
using API.Data.Requests;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository users, IMapper mapper) : ApiControllerBase {
    
  [HttpGet("all")]
  public async Task<ActionResult<IEnumerable<SimpleUser>>> GetUsers() 
    => Ok(await users.GetSimpleUsersAsync());

  [HttpGet("{id:int}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(uint id) {
    var user = await users.GetDbUserAsync(id);
    if(user == null) return NotFound();
    return Ok(await users.GetSimpleUserAsync(id));
  }
  
  [HttpGet("{username}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(string username) {
    var user = await users.GetDbUserAsync(username);
    return user == null ? NotFound() : Ok(await users.GetSimpleUserAsync(username));
  }

  [HttpPut]
  public async Task<ActionResult> UpdateUser(UpdateUserRequest request) {
    var username = User.FindFirst(JwtRegisteredClaimNames.Nickname)?.Value;
    if (username is null) return BadRequest("Something is wrong. Try logging out and logging back in.");
    
    var user = await users.GetDbUserAsync(username);
    if (user is null) return BadRequest("Could not find you in the database. How did you do that?");

    mapper.Map(request, user);

    return await users.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to update the user.");
  }
}
