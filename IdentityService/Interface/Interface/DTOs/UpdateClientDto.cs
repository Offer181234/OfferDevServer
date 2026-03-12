using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTOs
{
    public class UpdateClientDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? TokenType { get; set; }
        public string? UserType { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? ClientSecret { get; set; }
    }
}
