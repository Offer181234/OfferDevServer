using Interface.DTOs;
using Interface.Interface;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ClaimServices : IClaimService
    {
        private readonly IGenericRepository<ClaimDto> _claimRepository;

        public ClaimServices(IGenericRepository<ClaimDto> claimRepository)
        {
            _claimRepository = claimRepository;
        }

        public async Task<Guid> AddClaim(ClaimDto claimDto)
        {
            if (claimDto == null)
                throw new ArgumentNullException(nameof(claimDto));

            var claim = new ClaimDto
            {
                Id = Guid.NewGuid(),
                Name = claimDto.Name,
                Permission = claimDto.Permission,
                Description = claimDto.Description,
                ApiUrl = claimDto.ApiUrl,
                IsActive = claimDto.IsActive,
                CreatedBy = claimDto.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = claimDto.ModifiedBy,
                ModifiedOn = DateTime.UtcNow
            };

            return await _claimRepository.CreateAsync(claim);
        }

        public async Task<ClaimDto> GetClaimById(Guid id)
        {
            var claim = await _claimRepository.FindByIdAsync(id);

            if (claim == null)
                return null;

            return new ClaimDto
            {
                Id = claim.Id,
                Name = claim.Name,
                Permission = claim.Permission,
                Description = claim.Description,
                ApiUrl = claim.ApiUrl,
                IsActive = claim.IsActive
            };
        }

        public async Task<IList<ClaimDto>> GetAllClaims()
        {
            var claims = await _claimRepository.GetAllAsync();

            return claims.Select(c => new ClaimDto
            {
                Id = c.Id,
                Name = c.Name,
                Permission = c.Permission,
                Description = c.Description,
                ApiUrl = c.ApiUrl,
                IsActive = c.IsActive
            }).ToList();
        }

        public async Task<ClaimDto> UpdateClaim(ClaimDto claimDto)
        {
            var claim = await _claimRepository.FindByIdAsync(claimDto.Id);

            if (claim == null)
                throw new Exception("Claim not found");

            claim.Name = claimDto.Name;
            claim.Permission = claimDto.Permission;
            claim.Description = claimDto.Description;
            claim.ApiUrl = claimDto.ApiUrl;
            claim.IsActive = claimDto.IsActive;
            claim.ModifiedOn = DateTime.UtcNow;
            claim.ModifiedBy = "1";

            await _claimRepository.UpdateAsync(claim);

            return claimDto;
        }

        public async Task<ClaimDto> UpdateClaimStatusById(Guid claimId, string status)
        {
            var claim = await _claimRepository.FindByIdAsync(claimId);

            if (claim == null)
                throw new Exception("Claim not found");

            claim.IsActive = status == "active";
            claim.ModifiedOn = DateTime.UtcNow;
            claim.ModifiedBy = "1";

            await _claimRepository.UpdateAsync(claim);

            return new ClaimDto
            {
                Id = claim.Id,
                Name = claim.Name,
                Permission = claim.Permission,
                Description = claim.Description,
                ApiUrl = claim.ApiUrl,
                IsActive = claim.IsActive
            };
        }

        public async Task<ClaimDto> DeleteClaim(Guid id)
        {
            var claim = await _claimRepository.FindByIdAsync(id);

            if (claim == null)
                throw new Exception("Claim not found");

            await _claimRepository.DeleteAsync(claim);

            return new ClaimDto
            {
                Id = claim.Id,
                Name = claim.Name
            };
        }
    }
}
