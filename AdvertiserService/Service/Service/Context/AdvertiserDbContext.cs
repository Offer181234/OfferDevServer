using Interface.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Service.Context
{
    public class AdvertiserDbContext : DbContext
    {
        public AdvertiserDbContext(DbContextOptions<AdvertiserDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdvertiserDto> Advertisers { get; set; }


    }
}
