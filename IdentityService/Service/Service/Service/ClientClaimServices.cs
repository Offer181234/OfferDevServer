using Interface.DTOs;
using Interface.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ClientClaimServices : IClientClaimService
    {
        private readonly IGenericRepository<ClientClaimDto> _clientClaimRepository;

        public ClientClaimServices(IGenericRepository<ClientClaimDto> clientClaimRepository)
        {
            _clientClaimRepository = clientClaimRepository;
        }

        public async Task<Guid> AddClientClaim(ClientClaimDto clientClaimDto)
        {
            if (clientClaimDto == null)
                throw new ArgumentNullException(nameof(clientClaimDto));

            var clientClaim = new ClientClaimDto
            {
                Id = Guid.NewGuid(),
                ClientId = clientClaimDto.ClientId,
                ClaimId = clientClaimDto.ClaimId,
                IsActive = clientClaimDto.IsActive,
                CreatedBy = clientClaimDto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            return await _clientClaimRepository.CreateAsync(clientClaim);
        }

        public async Task<ClientClaimDto> GetClientClaimById(Guid id)
        {
            var clientClaim = await _clientClaimRepository.FindByIdAsync(id);

            if (clientClaim == null)
                return null;

            return new ClientClaimDto
            {
                Id = clientClaim.Id,
                ClientId = clientClaim.ClientId,
                ClaimId = clientClaim.ClaimId,
                IsActive = clientClaim.IsActive
            };
        }

        public async Task<IList<ClientClaimDto>> GetAllClientClaims()
        {
            var clientClaims = await _clientClaimRepository.GetAllAsync();

            return clientClaims.Select(c => new ClientClaimDto
            {
                Id = c.Id,
                ClientId = c.ClientId,
                ClaimId = c.ClaimId,
                IsActive = c.IsActive
            }).ToList();
        }

        public async Task<ClientClaimDto> UpdateClientClaim(ClientClaimDto clientClaimDto)
        {
            var clientClaim = await _clientClaimRepository.FindByIdAsync(clientClaimDto.Id);

            if (clientClaim == null)
                throw new Exception("Client Claim not found");

            clientClaim.ClientId = clientClaimDto.ClientId;
            clientClaim.ClaimId = clientClaimDto.ClaimId;
            clientClaim.IsActive = clientClaimDto.IsActive;
            clientClaim.ModifiedOn = DateTime.UtcNow;
            clientClaim.ModifiedBy = "1";

            await _clientClaimRepository.UpdateAsync(clientClaim);

            return clientClaimDto;
        }

        public async Task<ClientClaimDto> UpdateClientClaimStatusById(Guid id, string status)
        {
            var clientClaim = await _clientClaimRepository.FindByIdAsync(id);

            if (clientClaim == null)
                throw new Exception("Client Claim not found");

            clientClaim.IsActive = status == "active";
            clientClaim.ModifiedOn = DateTime.UtcNow;
            clientClaim.ModifiedBy = "1";

            await _clientClaimRepository.UpdateAsync(clientClaim);

            return new ClientClaimDto
            {
                Id = clientClaim.Id,
                ClientId = clientClaim.ClientId,
                ClaimId = clientClaim.ClaimId,
                IsActive = clientClaim.IsActive
            };
        }

        public async Task<ClientClaimDto> DeleteClientClaim(Guid id)
        {
            var clientClaim = await _clientClaimRepository.FindByIdAsync(id);

            if (clientClaim == null)
                throw new Exception("Client Claim not found");

            await _clientClaimRepository.DeleteAsync(clientClaim);

            return new ClientClaimDto
            {
                Id = clientClaim.Id,
                ClientId = clientClaim.ClientId,
                ClaimId = clientClaim.ClaimId
            };
        }
    }
}