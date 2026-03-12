using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string message);
    }
}
