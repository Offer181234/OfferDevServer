using Interface.DTOs;
namespace Interface.Interface
{
    public interface IUserService
    {
        Task<bool> Login(string email, string password);
        Task<bool> ForgotPassword(ForgotPasswordDto dto);
        Task<List<UserDto>> GetAllUsers();

        Task<UserDto?> GetUserById(Guid id);

        Task<UserDto> CreateUser(UserDto user);

        Task<UserDto?> UpdateUser(Guid id, UpdateUser user);

        Task<bool> DeleteUser(Guid id);
        Task<bool> VerifyOtp(VerifyOtpDto dto);
        Task<bool> ChangePassword(ChangePasswordDto dto);
    }
}
