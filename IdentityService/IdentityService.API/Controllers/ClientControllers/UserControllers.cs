using Interface.DTOs;
using Interface.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers;

namespace IdentityService.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.Login(email, password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUser dto)
        {
            var user = await _userService.UpdateUser(id, dto);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var result = await _userService.ForgotPassword(dto);

            if (!result)
                return NotFound("User not found");

            return Ok("OTP Sent To Email");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var result = await _userService.VerifyOtp(dto);

            if (!result)
                return BadRequest("Invalid OTP");

            return Ok("OTP Verified");
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _userService.ChangePassword(dto);

            if (!result)
                return BadRequest("User not found");

            return Ok("Password Changed Successfully");
        }
        // GET api/users
        [HttpGet]
        //[Authorize(Policy = PermissionRole.ADMIN)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Create(UserDto dto)
        {
            var user = await _userService.CreateUser(dto);
            return Ok(user);
        }

        // PUT api/users/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, UpdateUser dto)
        //{
        //    var user = await _userService.UpdateUser(id, dto);

        //    if (user == null)
        //        return NotFound("User not found");

        //    return Ok(user);
        //}

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUser(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User deleted successfully");
        }
    }

}