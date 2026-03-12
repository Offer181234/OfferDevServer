using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Claims
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
