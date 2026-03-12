using Interface.DTOs;
using Interface.Interface;
using Microsoft.EntityFrameworkCore;
using Service.Context;
using Service.Model;
using System.Security.Cryptography;

namespace Service.Service
{
    public class ClientService : IClientService
    {
        private readonly IdentityDbContext _context;
        private const int ClientSecretLength = 32;
        public IGenericRepository<ClientDto> _genericRepository { get; }

        public ClientService(IdentityDbContext context, IGenericRepository<ClientDto> genericRepository)
        {
            _context = context;
            _genericRepository = genericRepository;
        }

        public async Task<List<ClientDto>> GetAllClients()
        {
            return await _context.Clients
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Description = c.Description,
                    TokenType = c.TokenType,
                    UserType = c.UserType,
                    Role = c.Role,
                    UserPrincipalName = c.UserPrincipalName,
                    Status = c.Status,
                    StatusMessage = c.StatusMessage,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedOn = c.ModifiedOn,
                    ClientSecret = c.ClientSecret
                })
                .ToListAsync();
        }

        public async Task<ClientDto?> GetClientById(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return null;

            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Description = client.Description,
                TokenType = client.TokenType,
                UserType = client.UserType,
                Role = client.Role,
                UserPrincipalName = client.UserPrincipalName,
                Status = client.Status,
                StatusMessage = client.StatusMessage,
                CreatedBy = client.CreatedBy,
                CreatedOn = client.CreatedOn,
                ModifiedBy = client.ModifiedBy,
                ModifiedOn = client.ModifiedOn,
                ClientSecret = client.ClientSecret
            };
        }

        public async Task<ClientDto> CreateClient(ClientDto dto)
        {
            var client = new ClientDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Description = dto.Description,
                TokenType = dto.TokenType,
                UserType = dto.UserType,
                Role = dto.Role,
                UserPrincipalName = dto.UserPrincipalName,
                Status = dto.Status,
                StatusMessage = dto.StatusMessage,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                ClientSecret = GenerateClientSecret()
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            dto.Id = client.Id;

            return dto;
        }
        private static string GenerateClientSecret()
        {
            string clientSecret = "";

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[ClientSecretLength];
                rng.GetBytes(randomBytes);
                clientSecret = Convert.ToBase64String(randomBytes);
            }

            EncryptService _encryptService = new EncryptService();
            string encryptedSecret = _encryptService.EncryptString(clientSecret);

            return encryptedSecret;
        }
        public async Task<ClientDto?> UpdateClient(Guid id, UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return null;

            client.Name = dto.Name;
            client.Email = dto.Email;
            client.Description = dto.Description;
            client.TokenType = dto.TokenType;
            client.UserType = dto.UserType;
            client.Role = dto.Role;
            client.Status = dto.Status;
            client.ClientSecret = dto.ClientSecret;
            client.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };
        }

        public async Task<ClientDto?> GetClientsByIdAndSecret(Guid id, string clientSecret)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Client ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("Client secret cannot be empty.");

            var client = await _genericRepository.FindClientClaimsByIdAsync(id);

            if (client != null)
            {
                EncryptService encryptService = new EncryptService();
                string decriptService = encryptService.DecryptString(client.ClientSecret);
                //if (decriptService != clientSecret)
                //{
                //    throw new UnauthorizedAccessException("The Client secret is invalid");
                //}
                return client;
            }
            return null;
        }
     
        public async Task<bool> DeleteClient(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}