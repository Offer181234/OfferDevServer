using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Client
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }

        public string? TokenType { get; set; }

        public string? UserType { get; set; }

        public string? Role { get; set; }

        public string? UserPrincipalName { get; set; }

        public string? Status { get; set; }

        public string? StatusMessage { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ClientSecret { get; set; }
    }
}
