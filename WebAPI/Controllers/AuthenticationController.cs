
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Model;
using WebAPI.Service.User;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        readonly IUserService  _userService;
        [HttpPost("login")]
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
                var loginResult = await _userService.AuthenticateUser(user.Email,user.Password);
                if (loginResult != null)
                {
                    var token=await _userService.CreateAuthToken(loginResult.Data.Id, loginResult.Data.GroupId);
                }
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                //var tokeOptions = new JwtSecurityToken(
                //    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                //    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                //    claims: new List<Claim>(),
                //    expires: DateTime.Now.AddMinutes(6),
                //    signingCredentials: signinCredentials
                //);
                //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                //return Ok(new JwtAuthResult { Token = tokenString });
                   return Ok();

            }
            return Unauthorized();
        }
    }
}
