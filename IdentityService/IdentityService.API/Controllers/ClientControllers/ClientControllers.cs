using Interface.DTOs;
using Interface.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers;

namespace IdentityService.API.Controllers.ClientControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: api/clients
        [HttpGet]
        [Authorize(Policy = PermissionRole.ADMIN)]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllClients();
            return Ok(clients);
        }

        // GET: api/clients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var client = await _clientService.GetClientById(id);

            if (client == null)
                return NotFound("Client not found");

            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<IActionResult> Create(ClientDto dto)
        {
            var client = await _clientService.CreateClient(dto);
            return Ok(client);
        }

        // PUT: api/clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateClientDto dto)
        {
            var client = await _clientService.UpdateClient(id, dto);

            if (client == null)
                return NotFound("Client not found");

            return Ok(client);
        }

        // DELETE: api/clients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _clientService.DeleteClient(id);

            if (!result)
                return NotFound("Client not found");

            return Ok("Client deleted successfully");
        }
    }
}

