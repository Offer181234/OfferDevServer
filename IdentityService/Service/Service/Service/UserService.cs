using Interface.DTOs;
using Interface.Interface;
using Microsoft.EntityFrameworkCore;
using Service.Context;

namespace Service.Service
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _context;
        EncryptService encryptService = new EncryptService();

        public UserService(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email && x.IsActive);

            if (user == null)
                return false;

            string decryptedPassword = encryptService.DecryptString(user.PasswordHash);

            if (decryptedPassword != password)
                return false;

            return true;
        }
        public async Task<bool> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

            if (user == null)
                return false;

            user.PasswordHash = encryptService.EncryptString(dto.NewPassword);
            user.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<UserDto>> GetAllUsers()
        {
            return await _context.Users
                .Where(x => x.IsActive)   // Only Active Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserById(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedOn = user.CreatedOn
            };
        }

        public async Task<UserDto> CreateUser(UserDto dto)
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = encryptService.EncryptString(dto.PasswordHash),
                Role = dto.Role,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            dto.Id = user.Id;

            return dto;
        }

        public async Task<UserDto?> UpdateUser(Guid id, UpdateUser dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
                return null;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.ModifiedBy;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
                return false;

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}