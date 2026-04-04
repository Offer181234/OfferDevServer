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
                .Where(x => x.IsActive) // optional filter
                .Select(x => new AdvertiserDto
                {
                    Id = x.Id,

                    // BASIC
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CompanyName = x.CompanyName,

                    // LOGIN
                    Email = x.Email,
                    PasswordHash = x.PasswordHash,

                    // RELATION
                    AccountManagerId = x.AccountManagerId,

                    // STATUS
                    Status = x.Status,
                    SendCredentials = x.SendCredentials,
                    IsActive = x.IsActive,

                    // PROFILE
                    MobileNumber = x.MobileNumber,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    ZipCode = x.ZipCode,

                    // ACCOUNT
                    PostbackIp = x.PostbackIp,
                    Whitelist = x.Whitelist,
                    AdditionalInfo = x.AdditionalInfo,
                    PrivateNote = x.PrivateNote,

                    // AUDIT
                    CreatedOn = x.CreatedOn,
                    ModifiedOn = x.ModifiedOn
                })
                .ToListAsync();
        }

        public async Task<AdvertiserDto?> GetAdvertiserById(int id)
        {
            var x = await _context.Advertisers.FindAsync(id);

            if (x == null) return null;

            return new AdvertiserDto
            {
                Id = x.Id,

                // BASIC
                FirstName = x.FirstName,
                LastName = x.LastName,
                CompanyName = x.CompanyName,

                // LOGIN
                Email = x.Email,
                PasswordHash = x.PasswordHash,

                // RELATION
                AccountManagerId = x.AccountManagerId,

                // STATUS
                Status = x.Status,
                SendCredentials = x.SendCredentials,
                IsActive = x.IsActive,

                // 🔥 PROFILE (NEW)
                MobileNumber = x.MobileNumber,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Country = x.Country,
                ZipCode = x.ZipCode,

                // 🔥 ACCOUNT (NEW)
                PostbackIp = x.PostbackIp,
                Whitelist = x.Whitelist,
                AdditionalInfo = x.AdditionalInfo,
                PrivateNote = x.PrivateNote,

                // AUDIT
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
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

        public async Task<UpdateAdvertiserDto?> UpdateAdvertiser(int id, UpdateAdvertiserDto dto)
        {
            var entity = await _context.Advertisers.FindAsync(id);

            if (entity == null) return null;

            //entity.FirstName = dto.FirstName;
            //entity.LastName = dto.LastName;
            //entity.CompanyName = dto.CompanyName;
            //entity.AccountManagerId = dto.AccountManagerId;
            entity.Status = dto.Status;
            //entity.IsActive = dto.IsActive;
            entity.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateAdvertiserDto
            {
                Id = entity.Id,
                Status = entity.Status
                //LastName = entity.LastName,
                //Email = entity.Email
            };
        }
        public async Task<UpdateAdvertiserDetailsDto?> UpdateAdvertiserDetails(int id, UpdateAdvertiserDetailsDto dto)
        {
            var entity = await _context.Advertisers.FindAsync(id);

            if (entity == null) return null;

            // PROFILE
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.CompanyName = dto.CompanyName;
            entity.MobileNumber = dto.MobileNumber;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.State = dto.State;
            entity.Country = dto.Country;
            entity.ZipCode = dto.ZipCode;

            // ACCOUNT
            entity.Email = dto.Email;
            entity.IsActive = dto.IsActive;
            entity.PostbackIp = dto.PostbackIp;
            entity.Whitelist = dto.Whitelist;
            entity.AdditionalInfo = dto.AdditionalInfo;
            entity.PrivateNote = dto.PrivateNote;
            entity.Status = dto.Status;

            // COMMON
            entity.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dto;
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