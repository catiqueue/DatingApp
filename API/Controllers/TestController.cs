using API.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TestController : ApiControllerBase {
  [HttpGet("hello")]
  public ActionResult<string> GetHello() => Ok("Hello World!");
  
  [HttpGet("exception")]
  public ActionResult GetError() => throw new Exception("A spooky exception!");

  [Authorize]
  [HttpGet("secret")]
  public ActionResult<string> GetSecret() => "A secret key";
  
  [HttpGet("not-found")]
  public ActionResult<User> GetNotFound() => NotFound();
  
  [HttpGet("bad-request")]
  public ActionResult<string> GetBadRequest() => BadRequest("This was not a good request.");
}
