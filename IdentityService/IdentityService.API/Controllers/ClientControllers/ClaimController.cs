using Interface.DTOs;
using Interface.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.API.Controllers.ClientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpPost]
        public async Task<IActionResult> AddClaim([FromBody] ClaimDto claimDto)
        {
            var id = await _claimService.AddClaim(claimDto);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaimById(Guid id)
        {
            var claim = await _claimService.GetClaimById(id);

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClaims()
        {
            var claims = await _claimService.GetAllClaims();
            return Ok(claims);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClaim([FromBody] ClaimDto claim)
        {
            var updatedClaim = await _claimService.UpdateClaim(claim);
            return Ok(updatedClaim);
        }

        [HttpPatch("{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            var claim = await _claimService.UpdateClaimStatusById(id, status);
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            var claim = await _claimService.DeleteClaim(id);
            return Ok(claim);
        }
    }
}
