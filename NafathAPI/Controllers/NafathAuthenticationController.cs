using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NafathAPI.Application.Nafath;
using NafathAPI.Domain.Nafath.Dto;

namespace NafathAPI.Controllers
    {
    [ApiController]
    [Authorize ( AuthenticationSchemes = "APIKey" )]
    public class NafathAuthenticationController : ApiControllerBase
        {
        /// <summary>
        /// Basic Auth
        /// </summary>
        /// <param name="nationalId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route ( "challenge" )]
        public async Task<IActionResult> SignIn ( NafathSignInRequest request , CancellationToken cancellationToken )
            {
            var signInCommand = new SignInCommand { Request = request };
            var signinResponse = await Mediator.Send ( signInCommand , cancellationToken );

            if ( !signinResponse.Succeeded )
                {
                return BadRequest ( signinResponse );
                }
            return Ok ( signinResponse );

            }
        /// <summary>
        /// Basic Auth
        /// </summary>
        /// <param name="transId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route ( "NafathCheckStatus" )]
        public async Task<IActionResult> NafathCheckStatus ( NafathCheckStatusRequest request , CancellationToken cancellationToken )
            {
            var checkStatusCommand = new CheckStatusCommand { Request = request };
            var response = await Mediator.Send ( checkStatusCommand , cancellationToken );
            if ( response.Succeeded )
                {
                return Ok ( response );
                }
            else
                {
                return BadRequest ( response );
                }
            }
        /// <summary>
        ///  Basic Auth
        /// </summary>
        /// <param name="callbackRequest"></param>
        /// <returns></returns>
        //Do not authorize this action, because after investigation we found out that Nafath team doesn't send any authorization header.
        [HttpPost]
        [Route ( "callback" )]
        public async Task<IActionResult> NafathCallback ( [FromBody] NafathCallbackRequest callbackRequest )
            {
            var command = new CallbackCommand { Request = callbackRequest };
            var response = await Mediator.Send ( command );
            if ( response.Succeeded )
                {
                return Ok ( response );
                }
            else
                {
                return BadRequest ( response );
                }
            }
        }
    }

