using Application.Accounts.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Post(CreateAccountRequest request)
    {
        var command = new CreateAccount.Command(request.Mail, request.Password);


        var activityFeature = HttpContext.Features.Get<IHttpActivityFeature>();

        var currentActivity = activityFeature?.Activity;

        // TODO : can be done via middleware, but should definitely add the user id to the Activity
        activityFeature?.Activity
            .AddBaggage("correlation.id", Guid.NewGuid().ToString());

        activityFeature?.Activity
            .AddBaggage("user.identification", request.Mail);

        var result = await _mediator.Send(command);

        var currentActivity2 = activityFeature?.Activity;
        return Ok(result);
    }
}

public record CreateAccountRequest(string Mail, string Password);