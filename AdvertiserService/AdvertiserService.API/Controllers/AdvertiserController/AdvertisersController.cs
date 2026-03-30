using Interface.DTOs;
using Interface.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdvertiserService.API.Controllers.AdvertiserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisersController : ControllerBase
    {
        private readonly IAdvertiserService _service;
        public AdvertisersController(IAdvertiserService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAdvertisers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetAdvertiserById(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertiserDto dto)
        {
            var result = await _service.CreateAdvertiser(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAdvertiserDto dto)
        {
            var result = await _service.UpdateAdvertiser(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("details/{id}")]
        public async Task<IActionResult> UpdateAdvertiserDetails(int id, UpdateAdvertiserDetailsDto dto)
        {
            var result = await _service.UpdateAdvertiserDetails(id, dto);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAdvertiser(id);
            if (!result) return NotFound();
            return Ok("Deleted Successfully");
        }
    }
}
