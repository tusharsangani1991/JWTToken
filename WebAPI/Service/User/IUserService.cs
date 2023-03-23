using WebAPI.Model;
using WebAPI.Utilities;

namespace WebAPI.Service.User
{
    public interface IUserService
    {
        Task<Result<Infrastructure.Tabels.UserTable>> AuthenticateUser(string emailAddress, string password);
        Task<Result<ApiJwtAuthResult>> CreateAuthToken(Guid memberId, Guid groupId);
    }
}
