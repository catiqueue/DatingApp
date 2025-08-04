using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AllowAnonymous]
public class TestController : ApiControllerBase {
  [HttpGet("hello")]
  public string Get() => "Hello world!";
}
