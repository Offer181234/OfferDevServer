using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class UserPageAccessDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid PermissionId { get; set; }
        public bool? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public string? PermissionName { get; set; }


    }
}
