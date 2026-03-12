
using Interface.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Principal;
using Interface.DTOs;

namespace IdentityService.API.Controllers.ClientControllers
{
    [ApiController]
    [Route("identity")]
    public class ApiSecurityController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ITokenService _tokenService;
        //private readonly ILogRequestService _logRequestService;
        private readonly ILogger<ApiSecurityController> _logger;

        public ApiSecurityController(
            IClientService clientService,
            ITokenService tokenService,
            //ILogRequestService logRequestService,
            ILogger<ApiSecurityController> logger)
        {
            _clientService = clientService;
            _tokenService = tokenService;
            //_logRequestService = logRequestService;
            _logger = logger;
        }

        //[SwaggerOperation(Tags = new[] { "Platform Security" })]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest(new { message = "Account details are required." });
            }

            if (!Guid.TryParse(account.ClientId, out Guid clientId))
            {
                return BadRequest(new { message = "Invalid ClientId format." });
            }

            var client = await _clientService.GetClientsByIdAndSecret(
                clientId,
                account.ClientSecret
            );

            //await _clientService.LogApiHitAsync(HttpContext, _logRequestService, _logger);

            if (client == null)
            {
                return Unauthorized(new { message = "Invalid client credentials." });
            }

            var token = await _tokenService.GetJwtToken(client, null, null);

            return Ok(token);
        }
    }
}