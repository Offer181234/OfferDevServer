using Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IClientClaimService
    {
        Task<Guid> AddClientClaim(ClientClaimDto clientClaimDto);

        Task<ClientClaimDto> GetClientClaimById(Guid id);

        Task<IList<ClientClaimDto>> GetAllClientClaims();

        Task<ClientClaimDto> UpdateClientClaim(ClientClaimDto clientClaim);

        Task<ClientClaimDto> UpdateClientClaimStatusById(Guid id, string status);

        Task<ClientClaimDto> DeleteClientClaim(Guid id);
    }
}