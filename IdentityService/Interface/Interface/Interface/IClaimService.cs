using Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IClaimService
    {
        Task<Guid> AddClaim(ClaimDto claimDto);

        Task<ClaimDto> GetClaimById(Guid id);

        Task<IList<ClaimDto>> GetAllClaims();

        Task<ClaimDto> UpdateClaim(ClaimDto claim);

        Task<ClaimDto> UpdateClaimStatusById(Guid claimId, string status);

        Task<ClaimDto> DeleteClaim(Guid id);
    }
}
