using Application.Features.Login.Commands.LoginJamaagent;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : ApiControllerBase
    {

        public AuthController()
        {

        }

        [HttpPost("agent/login")]
        public async Task<IActionResult> Login([FromServices] IValidator<LoginJamaaAgentCommand> validator,[FromBody] LoginJamaaAgentCommand request)
        {
            var validRes = await validator.ValidateAsync(request);

            if (!validRes.IsValid)
                return NotValidRequest(validRes);

            var res = await Mediator.Send(request);
            if (!res.success)
                return Unauthorized(res);
           
            return Ok(res);
        }

    }
}
