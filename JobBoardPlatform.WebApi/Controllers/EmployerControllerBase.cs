using JobBoardPlatform.Buisiness.Common.Exceptions;

namespace JobBoardPlatform.WebApi.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Employer")]
public abstract class EmployerControllerBase : ControllerBase
{
    protected int GetEmployerId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    protected void EnsureApproved()
    {
        if (User.FindFirstValue("isApproved") != "True")
            throw new ForbiddenAccessException("حساب کارفرمایی شما هنوز تأیید نشده است");
    }
}