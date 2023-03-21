using WebAPI.Utilities;

namespace WebAPI
{
    public interface IConfig
    {
        byte[] ApiAuthEncryptionKey { get; }
    }

    public class Config : IConfig
    {
        public Config(IConfiguration config)
        {
            ApiAuthEncryptionKey = Safe64.Decode(config["ApiAuthKey"]);
        }
        public byte[] ApiAuthEncryptionKey { get; }
    }
}
