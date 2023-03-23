using SecurityDriven.Inferno;
using System.Security.Claims;
using WebAPI.Utilities;

namespace WebAPI
{
    public class ApiAuthToken
    {
        public ApiAuthToken(Guid tokenId)
        {
            TokenId = tokenId;
            
        }

        public Guid TokenId { get; }
        //public Guid GroupId { get; }

        static CryptoRandom m_rng = new CryptoRandom();

        const int SaltLength = 16;

        public static (ApiAuthToken AuthToken, IToken Token) From(ClaimsPrincipal user)
        {
            if (user?.HasClaim(v => v.Type == ClaimTypes.NameIdentifier) != true) return (null, null);
            var val = user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.NameIdentifier).Value?.Split('~');
            if (val?.Length != 3) return (null, null);
            var id = val[0].ToGuid();
            var grpId = val[1].ToShortGuid();
            var usrId = val[2].ToGuid();
            if (id == null || grpId == null || usrId == null) return (null, null);
            return (new ApiAuthToken(id.Value), new Token(id.Value, usrId.Value, grpId.Value));
        }

        public static ApiAuthToken Parse(byte[] masterKey, string rawToken)
        {
            var bytes = Safe64.Decode(rawToken, nullOnError: true);
            if (bytes == null || bytes.Length <= SaltLength) return null;

            var decrypted = SuiteB.Decrypt(masterKey, new ArraySegment<byte>(bytes, SaltLength, bytes.Length - SaltLength), new ArraySegment<byte>(bytes, 0, SaltLength));
            if (decrypted == null || decrypted.Length != 32) return null;

            var tokenId = new Guid(new ArraySegment<byte>(decrypted, 0, 16));
            var grpId = new Guid(new ArraySegment<byte>(decrypted, 16, 16));

            return new ApiAuthToken(tokenId);
        }

        public string ToString(byte[] masterKey)
        {
            var bytes = new byte[32];
            Buffer.BlockCopy(TokenId.ToByteArray(), 0, bytes, 0, 16);
           // Buffer.BlockCopy(GroupId.ToByteArray(), 0, bytes, 16, 16);

            var salt = m_rng.NextBytes(SaltLength);

            var enc = SuiteB.Encrypt(masterKey, bytes, salt);
            if (enc == null) return null;

            var salted = new byte[enc.Length + SaltLength];
            Buffer.BlockCopy(salt, 0, salted, 0, SaltLength);
            Buffer.BlockCopy(enc, 0, salted, SaltLength, enc.Length);

            return Safe64.Encode(salted);
        }

        public Claim ToClaim(Guid memberId)
        {
            return new Claim(ClaimTypes.NameIdentifier, TokenId + "~" + "~" + memberId);
        }
    }
}
