using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers;

[Authorize(Roles = "Admin")]
public abstract class AdminControllerBase : ControllerBase
{
}