using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OfferService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {

        // GET api/offer/test
        [HttpGet("test")]
        public IActionResult TestAPI()
        {
            return Ok("Offer Service API is Working...");
        }


        // GET api/offer/getall
        [HttpGet("getall")]
        public IActionResult GetAllOffers()
        {
            var offers = new List<object>
            {
                new { Id = 1, Name = "Offer 1", Discount = "10%" },
                new { Id = 2, Name = "Offer 2", Discount = "20%" },
                new { Id = 3, Name = "Offer 3", Discount = "30%" }
            };

            return Ok(offers);
        }


        // GET api/offer/getbyid/1
        [HttpGet("getbyid/{id}")]
        public IActionResult GetOfferById(int id)
        {
            var offer = new
            {
                Id = id,
                Name = "Offer " + id,
                Discount = "15%"
            };

            return Ok(offer);
        }


        // POST api/offer/create
        [HttpPost("create")]
        public IActionResult CreateOffer([FromBody] object offer)
        {
            return Ok(new
            {
                Message = "Offer Created Successfully",
                Data = offer
            });
        }


        // PUT api/offer/update/1
        [HttpPut("update/{id}")]
        public IActionResult UpdateOffer(int id, [FromBody] object offer)
        {
            return Ok(new
            {
                Message = "Offer Updated Successfully",
                Id = id,
                Data = offer
            });
        }


        // DELETE api/offer/delete/1
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteOffer(int id)
        {
            return Ok(new
            {
                Message = "Offer Deleted Successfully",
                Id = id
            });
        }

    }
}
