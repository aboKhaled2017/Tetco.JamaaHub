using Domain.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AgentOperationsController: ApiControllerBase
    {

        //[HttpPost("insert")]
        //[Authorize(Policy = Policies.CanInsertStudents)]
        //public async Task<IActionResult> InsertBatch([FromServices] IValidator<InsertStudentBatchCommand> validator, [FromBody] InsertStudentBatchCommand request)
        //{
        //    var validRes = await validator.ValidateAsync(request);

        //    if (!validRes.IsValid)
        //        return NotValidRequest(validRes);

        //    var res = await Mediator.Send(request);

        //    return Ok(res);
        //}
    }
}
