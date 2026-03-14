using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class UpdateUserPermissionDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? PermissionName { get; set; }   


        public string? Role { get; set; }

        public List<UserPageAccessDto> Permissions { get; set; }
    }
}
