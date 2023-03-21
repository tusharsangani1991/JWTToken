using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Tabels;
using WebAPI.Model;
using WebAPI.Utilities;
using WebAPI.Utilities.Jwt;

namespace WebAPI.Service.User
{
    public class UserService : IUserService
    {
        public UserService(IJwtAuthManager jwtAuthManager, IConfig config, DbContextEx context)
        {
            _jwtAuthManager = jwtAuthManager;
            _context = context;
            m_config = config;
        }
        readonly IConfig m_config;
        readonly IJwtAuthManager _jwtAuthManager;
        readonly DbContextEx _context;



        public async Task<Result<Infrastructure.Tabels.User>> AuthenticateUser(string emailAddress, string password)
        {
            var login = await _context.Users.FirstOrDefaultAsync(v => v.Email == emailAddress && v.Password == password);
            return login;
        }
        public Task<Result<ApiJwtAuthResult>> CreateAuthToken(Guid memberId, Guid groupId)
        {
            return GenerateAuthToken(memberId, groupId);
        }
        internal Task<Result<ApiJwtAuthResult>> GenerateAuthToken(Guid memberid, Guid groupId)
        {
            var result = new ApiJwtAuthResult();

            var tokenId = Guid.NewGuid();
            var token = new ApiAuthToken(tokenId, groupId);
            var encryptedToken = token.ToString(m_config.ApiAuthEncryptionKey);

            result.EncryptedToken = encryptedToken;
            ApiTokenTable apiToken = new ApiTokenTable();

            var jwtToken = _jwtAuthManager.GenerateJwtToken(tokenId.ToString(), encryptedToken);
            apiToken.Id = tokenId;
            apiToken.AccessToken = result.AccessToken = jwtToken.AccessToken;
            apiToken.RefreshToken = result.RefreshToken = jwtToken.RefreshToken?.TokenString;
            apiToken.RefreshExpiryTime = result.RefreshExpiryTime = jwtToken.RefreshToken.ExpireAt;
            apiToken.UserGuid = memberid;
            apiToken.GroupId = groupId;
            _context.ApiTokens.Add(apiToken);

            //tokenId = m_broker.Request(new GetOrCreateToken(appKey, tokenId, memberid, groupId, result.EncryptedToken, result.AccessToken, result.RefreshToken, result.RefreshExpiryTime)).Id;
            //  m_logger.Log(LogLevel.Debug, $"New token generated for member {memberid}, account {accountId}, token id {tokenId}");

            return Task.FromResult<Result<ApiJwtAuthResult>>(result);
        }
    }
}
