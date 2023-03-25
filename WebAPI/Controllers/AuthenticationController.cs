
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Tabels;
using WebAPI.Model;
using WebAPI.Service.User;
using WebAPI.Utilities;
using WebAPI.Utilities.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {

        public AuthenticationController(DbContextEx context, IConfig config, IJwtAuthManager jwtAuthManager)
        {

            _context = context;
            m_config = config;
            _jwtAuthManager = jwtAuthManager;

        }
        readonly DbContextEx _context;
        readonly IUserService _userService;
        readonly IConfig m_config;
        readonly IJwtAuthManager _jwtAuthManager;


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] ApiAuthLoginRequestModel user)
        {
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            else if (user.Email.IsNullOrBlank())
            {
                return BadRequest("Invalid Email Id");

            }
            else if (user.Password.IsNullOrBlank())
            {
                return BadRequest("Please provide user Password");

            }
            else
            {
                var loginResult = (from u in _context.Users
                                   join ur in _context.UserRoles
                                   on u.Id equals ur.UserId
                                   join role in _context.Roles
                                   on ur.RoleId equals role.Id
                                   where u.Email == user.Email && u.Password == user.Password
                                   select new
                                   {
                                       user = u,
                                       roles = role
                                   }).FirstOrDefault();
                //  _context.Entry(loginResult.roles).Collection(v => v.UserRoles).Load();
                //var loginResult = await _context.Users.FirstOrDefaultAsync(v => v.Email == user.Email && v.Password == user.Password);
                if (loginResult != null)
                {
                    var roles = new List<string>();
                    roles.Add(loginResult.roles.Role);
                    var token = await GenerateAuthToken(loginResult.user.Id, roles);

                    return Ok(new JwtAuthResult { AccessToken = token.Data.AccessToken, RefreshToken = (new RefreshToken { TokenString = token.Data.RefreshToken, ExpireAt = token.Data.RefreshExpiryTime }) });
                }

            }
            return Unauthorized();
        }

        internal Task<Result<ApiJwtAuthResult>> GenerateAuthToken(Guid memberid, List<string> role)
        {
            var result = new ApiJwtAuthResult();

            var tokenId = Guid.NewGuid();
            var token = new ApiAuthToken(tokenId);
            var encryptedToken = token.ToString(m_config.ApiAuthEncryptionKey);

            result.EncryptedToken = encryptedToken;
            ApiTokenTable apiToken = new ApiTokenTable();

            var jwtToken = _jwtAuthManager.GenerateJwtToken(tokenId.ToString(), encryptedToken,role);
            apiToken.Id = tokenId;
            apiToken.AccessToken = result.AccessToken = jwtToken.AccessToken;
            apiToken.RefreshToken = result.RefreshToken = jwtToken.RefreshToken?.TokenString;
            apiToken.RefreshExpiryTime = result.RefreshExpiryTime = jwtToken.RefreshToken.ExpireAt;
            apiToken.UserGuid = memberid;
            //  apiToken.GroupId = groupId;
            _context.ApiTokens.Add(apiToken);
            _context.SaveChangesAsync();

            return Task.FromResult<Result<ApiJwtAuthResult>>(result);
        }

    }
}
