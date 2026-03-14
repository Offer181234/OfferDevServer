using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class PermissionDto
    {
        public Guid Id { get; set; }

        public string? PermissionName { get; set; }

        public string? Description { get; set; }
    }
}
