using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface.DTOs;

namespace Interface.Interface
{
    public interface ITokenService
    {
        //test
        Task<string> GetJwtToken(ClientDto client, DateTime? expiresIn, string platformToken);
    }
}
