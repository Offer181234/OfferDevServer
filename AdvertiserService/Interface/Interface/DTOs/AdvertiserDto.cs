using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class AdvertiserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? CompanyName { get; set; }

        public string Email { get; set; } = null!;
        //public string Password { get; set; } = null!;

        public Guid? AccountManagerId { get; set; }

        public string Status { get; set; } = "ACTIVE";
        public bool SendCredentials { get; set; }

        public bool IsActive { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string PasswordHash { get; set; } = null!;

    }

    public class UpdateAdvertiserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? CompanyName { get; set; }
        public Guid? AccountManagerId { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public bool IsActive { get; set; }
    }
}
