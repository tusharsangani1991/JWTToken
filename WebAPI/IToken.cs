using System.Text.RegularExpressions;

namespace WebAPI
{
    public interface IToken
    {
        Guid Id { get; }
        Guid UserGuid { get; }

        public Guid GroupId { get; }

        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime RefreshExpiryTime { get; }
    }

    public class Token : IToken
    {
        public Token(Guid id, Guid userId, Guid groupId)
        {
            UserGuid = userId;
            GroupId = groupId;
            Id = id;
        }

        public Token(Guid userId, Guid groupId, Guid id, string accessToken, string refreshToken, DateTime refreshExpiryTime)
        {
            Id = id;
            UserGuid = userId;
            GroupId = groupId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshExpiryTime = refreshExpiryTime;
        }

        public Guid Id { get; }
        public Guid UserGuid { get; }
        public Guid GroupId { get; }

        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime RefreshExpiryTime { get; }
    }
}
