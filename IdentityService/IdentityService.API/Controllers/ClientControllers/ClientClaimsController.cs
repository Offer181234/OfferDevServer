using Interface.DTOs;
using Interface.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers.ClientControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientClaimsController : ControllerBase
    {
        private readonly IClientClaimService _clientClaimService;

        public ClientClaimsController(IClientClaimService clientClaimService)
        {
            _clientClaimService = clientClaimService;
        }

        [HttpPost]
        public async Task<IActionResult> AddClientClaim([FromBody] ClientClaimDto clientClaimDto)
        {
            var id = await _clientClaimService.AddClientClaim(clientClaimDto);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientClaimById(Guid id)
        {
            var clientClaim = await _clientClaimService.GetClientClaimById(id);

            if (clientClaim == null)
                return NotFound();

            return Ok(clientClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientClaims()
        {
            var clientClaims = await _clientClaimService.GetAllClientClaims();
            return Ok(clientClaims);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClientClaim([FromBody] ClientClaimDto clientClaim)
        {
            var updatedClientClaim = await _clientClaimService.UpdateClientClaim(clientClaim);
            return Ok(updatedClientClaim);
        }

        [HttpPatch("{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            var clientClaim = await _clientClaimService.UpdateClientClaimStatusById(id, status);
            return Ok(clientClaim);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientClaim(Guid id)
        {
            var clientClaim = await _clientClaimService.DeleteClientClaim(id);
            return Ok(clientClaim);
        }
    }
}