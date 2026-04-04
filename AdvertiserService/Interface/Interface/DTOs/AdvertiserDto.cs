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

        // BASIC
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        // LOGIN
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        // RELATION
        public Guid? AccountManagerId { get; set; }

        // STATUS
        public string? Status { get; set; }
        public bool? SendCredentials { get; set; }
        public bool IsActive { get; set; }

        // PROFILE (NEW)
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }

        // ACCOUNT (NEW)
        public string? PostbackIp { get; set; }
        public string? Whitelist { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? PrivateNote { get; set; }

        // AUDIT
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class UpdateAdvertiserDto
    {
        //public string FirstName { get; set; } = null!;
        //public string LastName { get; set; } = null!;
        //public string? CompanyName { get; set; }
        //public Guid? AccountManagerId { get; set; }
        public int Id { get; set; }
        public string? Status { get; set; } 
        public bool IsActive { get; set; }
    }
    public class UpdateAdvertiserDetailsDto
    {
        public int Id { get; set; }

        // Profile
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }

        // Account
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string? PostbackIp { get; set; }
        public string? Whitelist { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? PrivateNote { get; set; }
        public string? Status { get; set; }

    }
}
