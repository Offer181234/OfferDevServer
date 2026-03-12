using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class ClientClaimDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Guid ClaimId { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        [JsonIgnore]
        public ClientDto? Client { get; set; }
    }
}
