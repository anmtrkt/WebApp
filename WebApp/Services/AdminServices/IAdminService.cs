using WebApp.DB;
using WebApp.Models.UsersModel;

namespace WebApp.Services.AdminServices
{
    public interface IAdminService
    {
        Task<bool> RoleMaking(Guid userId, int role);
        Task<List<UserModel>> AllUsers();
        Task<object> OneUserById(Guid UserId);
        Task<object> OneUserByEmail(string Email);
        Task<bool> DeleteUser(Guid UserId);
    }
}
