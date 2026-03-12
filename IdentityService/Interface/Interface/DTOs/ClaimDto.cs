using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class ClaimDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }

        public string? Permission { get; set; }

        public string? Description { get; set; }

        public string? ApiUrl { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
