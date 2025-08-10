using System.Collections;

using API.Data;
using API.Data.DTOs;
using API.Data.Repositories;
using API.Entities;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository users) : ApiControllerBase {
    
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
}
