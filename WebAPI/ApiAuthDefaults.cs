using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WebAPI.Infrastructure;
using WebAPI.Utilities;
using WebAPI.Utilities.Jwt;

namespace WebAPI
{
    public class ApiAuthDefaults
    {
        public const string Scheme = "ApiAuth";

    }
    public class ApiAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, IConfig config, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IJwtAuthManager jwtAuthManager, DbContextEx dbContext)
            : base(options, logger, encoder, clock)
        {
            m_config = config;
            _context = dbContext;
            _jwtAuthManager = jwtAuthManager;
        }

        readonly IConfig m_config;
        readonly IJwtAuthManager _jwtAuthManager;
        readonly DbContextEx _context;
        protected override Task<AuthenticateResult> HandleAuthenticateAsync() => Task.FromResult(HandleAuthenticate());
        private AuthenticateResult HandleAuthenticate()
        {
            string header = Context.Request.Headers[HeaderNames.Authorization];

            return HandleJwtAuthenticate(header);
        }
        private AuthenticateResult HandleJwtAuthenticate(string header)
        {
            string headerToken = header?.Split(" ").Last();
            if (headerToken.IsNullOrBlank()) return AuthenticateResult.NoResult();

            var (rawJwtToken, isValid) = _jwtAuthManager.IsTokenValid(headerToken);
            if (isValid)
            {
                var token = ApiAuthToken.Parse(m_config.ApiAuthEncryptionKey, rawJwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash).Value);
                if (token == null) return AuthenticateResult.Fail($"Invalid token; Parse failed for {rawJwtToken}");

                var principal = ResolveTokenPrincipal(token);
                if (principal == null) return AuthenticateResult.Fail($"Invalid token; Unable to resolve principal on token {token.TokenId}");

                return AuthenticateResult.Success(new AuthenticationTicket(principal, ApiAuthDefaults.Scheme));
            }

            return AuthenticateResult.NoResult();
        }
        private ClaimsPrincipal ResolveTokenPrincipal(ApiAuthToken token)
        {
            var dbToken = _context.ApiTokens.FirstOrDefault(v => v.Id == token.TokenId); //m_broker.Request(new GetTokenById(token.TokenId, token.AccountId.Value));
            if (dbToken == null) return null;

            var claims = new Claim[] { token.ToClaim(dbToken.UserGuid) };
            var identity = new ClaimsIdentity(claims, ApiAuthDefaults.Scheme);
            return new ClaimsPrincipal(identity);
        }

    }
}
