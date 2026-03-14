using Interface.DTOs;
namespace Interface.Interface
{
    public interface IUserService
    {
        Task<LoginResponseDto?> Login(string email, string password);
        Task<UserDto?> UpdateUser(Guid id, UpdateUser dto);
        Task<bool> ForgotPassword(ForgotPasswordDto dto);
        Task<List<UserDto>> GetAllUsers();

        Task<UpdateUserPermissionDto?> GetUserById(Guid id);

        Task<UserDto> CreateUser(UserDto user);

        //Task<UserDto?> UpdateUser(Guid id, UpdateUser user);

        Task<bool> DeleteUser(Guid id);
        Task<bool> VerifyOtp(VerifyOtpDto dto);
        Task<bool> ChangePassword(ChangePasswordDto dto);
    }
}
