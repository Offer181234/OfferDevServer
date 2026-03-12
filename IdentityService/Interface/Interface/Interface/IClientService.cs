using Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllClients();

        Task<ClientDto?> GetClientById(Guid id);

        Task<ClientDto> CreateClient(ClientDto client);

        Task<ClientDto?> UpdateClient(Guid id, UpdateClientDto client);

        Task<bool> DeleteClient(Guid id);
        Task<ClientDto?> GetClientsByIdAndSecret(Guid id, string clientSecret);
    }
}