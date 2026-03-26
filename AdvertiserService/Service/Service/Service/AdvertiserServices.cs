using Interface.DTOs;
using Interface.Interface;
using Microsoft.EntityFrameworkCore;
using Service.Context;

namespace Service.Service
{
    public class AdvertiserServices : IAdvertiserService
    {
        private readonly AdvertiserDbContext _context;
        EncryptService encryptService = new EncryptService();

        public AdvertiserServices(AdvertiserDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdvertiserDto>> GetAllAdvertisers()
        {
            return await _context.Advertisers
                .Where(x => x.IsActive)
                .Select(x => new AdvertiserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CompanyName = x.CompanyName,
                    AccountManagerId = x.AccountManagerId,
                    Status = x.Status,
                    SendCredentials = x.SendCredentials,
                    IsActive = x.IsActive
                }).ToListAsync();
        }

        public async Task<AdvertiserDto?> GetAdvertiserById(int id)
        {
            var x = await _context.Advertisers.FindAsync(id);

            if (x == null) return null;

            return new AdvertiserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                CompanyName = x.CompanyName,
                AccountManagerId = x.AccountManagerId,
                Status = x.Status,
                SendCredentials = x.SendCredentials,
                IsActive = x.IsActive
            };
        }

        public async Task<AdvertiserDto> CreateAdvertiser(AdvertiserDto dto)
        {
            var entity = new AdvertiserDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                CompanyName = dto.CompanyName,
                AccountManagerId = dto.AccountManagerId,
                Status = dto.Status,
                SendCredentials = dto.SendCredentials,
                IsActive = true,
                PasswordHash = encryptService.EncryptString(dto.PasswordHash),
                CreatedOn = DateTime.UtcNow
            };

            _context.Advertisers.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<AdvertiserDto?> UpdateAdvertiser(int id, UpdateAdvertiserDto dto)
        {
            var entity = await _context.Advertisers.FindAsync(id);

            if (entity == null) return null;

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.CompanyName = dto.CompanyName;
            entity.AccountManagerId = dto.AccountManagerId;
            entity.Status = dto.Status;
            entity.IsActive = dto.IsActive;
            entity.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new AdvertiserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email
            };
        }

        public async Task<bool> DeleteAdvertiser(int id)
        {
            var entity = await _context.Advertisers.FindAsync(id);

            if (entity == null) return false;

            entity.IsActive = false;
            entity.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}