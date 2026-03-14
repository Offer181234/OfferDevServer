using Interface.DTOs;
using Microsoft.EntityFrameworkCore;
using Service.Model;

namespace Service.Context
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserDto> Users { get; set; }

        public DbSet<ClientDto> Clients { get; set; }

        public DbSet<ClaimDto> Claims { get; set; }

        public DbSet<ClientClaimDto> ClientClaims { get; set; }
        public DbSet<UserPermissionDto> UserPermissions { get; set; }
        public DbSet<PermissionDto> Permissions { get; set; }

    }
}
