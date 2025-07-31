using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class TestController : ControllerBase {
    [HttpGet("hello")]
    public string Get() => "Hello world!";
  }
}
