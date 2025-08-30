using API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter<UserActivityTracker>]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase;
