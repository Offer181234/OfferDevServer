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
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;


        public UserService(IdentityDbContext context, IEmailService emailService, ITokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto?> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email && x.IsActive);

            if (user == null)
                return null;

            string decryptedPassword = encryptService.DecryptString(user.PasswordHash);

            if (decryptedPassword != password)
                return null;

            // GET USER PERMISSIONS
            var permissions = await (
                from up in _context.UserPermissions
                join p in _context.Permissions
                on up.PermissionId equals p.Id
                where up.UserId == user.Id && up.DeletedOn == true
                select p.PermissionName
            ).ToListAsync();

            // Prepare client dto for JWT
            var client = new ClientDto
            {
                Id = user.Id,
                Name = user.FirstName,
                Role = user.Role,
                TokenType = "User",
                UserType = user.Role,
                clientClaimDtos = permissions.Select(p => new ClientClaimDto
                {
                    ClaimId = Guid.NewGuid()
                }).ToList()
            };

            var token = await _tokenService.GetJwtToken(client, null, "");

            return new LoginResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Permissions = permissions,
                Token = token
            };
        }
        public async Task<UserDto?> UpdateUser(Guid id, UpdateUser dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
                return null;

            // Update User Info
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = dto.ModifiedBy;

            // Get old permissions
            var oldPermissions = await _context.UserPermissions
                .Where(x => x.UserId == id && x.DeletedOn == true)
                .ToListAsync();

            // Soft delete old permissions
            foreach (var item in oldPermissions)
            {
                item.DeletedOn = false;
                item.DeletedBy = dto.ModifiedBy;
            }

            // Add new permissions
            if (dto.PermissionIds != null && dto.PermissionIds.Any())
            {
                var newPermissions = dto.PermissionIds.Select(p => new UserPermissionDto
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    PermissionId = p,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = dto.ModifiedBy,
                    DeletedOn = true,
                    DeletedBy = null
                });

                await _context.UserPermissions.AddRangeAsync(newPermissions);
            }

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<bool> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

            if (user == null)
                return false;

            var otp = new Random().Next(100000, 999999).ToString();

            user.ResetOtp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();

            // Email send logic
            await _emailService.SendEmail(user.Email, "Password Reset OTP", $"Your OTP is {otp}");

            return true;
        }
        public async Task<bool> VerifyOtp(VerifyOtpDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

            if (user == null)
                return false;

            if (user.ResetOtp != dto.Otp || user.OtpExpiry < DateTime.UtcNow)
                return false;

            return true;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

            if (user == null)
                return false;

            user.PasswordHash = encryptService.EncryptString(dto.NewPassword);
            user.ModifiedOn = DateTime.UtcNow;

            user.ResetOtp = null;
            user.OtpExpiry = null;

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

        public async Task<UpdateUserPermissionDto?> GetUserById(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (user == null)
                return null;

            var permissions = await (
                from up in _context.UserPermissions
                join p in _context.Permissions
                on up.PermissionId equals p.Id
                where up.UserId == user.Id && up.DeletedOn == true
                select new UserPageAccessDto
                {
                    Id = up.Id,
                    UserId = up.UserId,
                    PermissionId = up.PermissionId,
                    PermissionName = p.PermissionName,   
                    DeletedOn = up.DeletedOn,
                    CreatedOn = up.CreatedOn,
                    CreatedBy = up.CreatedBy,
                    DeletedBy = up.DeletedBy
                }
            ).ToListAsync();

            return new UpdateUserPermissionDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Permissions = permissions
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

        //public async Task<UserDto?> UpdateUser(Guid id, UpdateUser dto)
        //{
        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        //    if (user == null)
        //        return null;

        //    user.FirstName = dto.FirstName;
        //    user.LastName = dto.LastName;
        //    user.Email = dto.Email;
        //    user.Role = dto.Role;
        //    user.IsActive = dto.IsActive;
        //    user.ModifiedOn = DateTime.UtcNow;
        //    user.ModifiedBy = dto.ModifiedBy;

        //    await _context.SaveChangesAsync();

        //    return new UserDto
        //    {
        //        Id = user.Id,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email
        //    };
        //}

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