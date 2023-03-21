using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Utilities;
using WebAPI.Utilities.Jwt;

namespace WebAPI.Service.User
{
    public class UserService
    {
        public UserService(IJwtAuthManager jwtAuthManager, IConfig config, DbContextClass context)
        {
            _jwtAuthManager = jwtAuthManager;
            _context = context;
            m_config = config;
        }
        readonly IConfig m_config;
        readonly IJwtAuthManager _jwtAuthManager;
        private readonly DbContextClass _context;


        internal Task<Result<ApiJwtAuthResult>> GenerateAuthToken(string appKey, Guid memberid, Guid groupId)
        {
            var result = new ApiJwtAuthResult();

            var tokenId = Guid.NewGuid();
            var token = new ApiAuthToken(tokenId, groupId);
            var encryptedToken = token.ToString(m_config.ApiAuthEncryptionKey);

            result.EncryptedToken = encryptedToken;

            var jwtToken = _jwtAuthManager.GenerateJwtToken(tokenId.ToString(), encryptedToken);
            result.AccessToken = jwtToken.AccessToken;
            result.RefreshToken = jwtToken.RefreshToken?.TokenString;
            result.RefreshExpiryTime = jwtToken.RefreshToken.ExpireAt;
            
            

            //tokenId = m_broker.Request(new GetOrCreateToken(appKey, tokenId, memberid, groupId, result.EncryptedToken, result.AccessToken, result.RefreshToken, result.RefreshExpiryTime)).Id;
            //  m_logger.Log(LogLevel.Debug, $"New token generated for member {memberid}, account {accountId}, token id {tokenId}");

            return Task.FromResult<Result<ApiJwtAuthResult>>(result);
        }
    }
}
