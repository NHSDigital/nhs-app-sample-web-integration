using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace nhsapp.sample.web.integration.Certificate
{
    public interface ISigning
    {
        RSAParameters GetRsaParameters(IKeyConfig keyConfig);
    }
    public class Signing : ISigning
    {
        public RSAParameters GetRsaParameters(IKeyConfig keyConfig)
        {
            using var reader = File.OpenText(keyConfig.KeyPath);

            var passwordFinder = new PasswordFinder(keyConfig.Password);
            var rsaKey = (RsaPrivateCrtKeyParameters) new PemReader(reader, passwordFinder).ReadObject();
            var rsaParameters = DotNetUtilities.ToRSAParameters(rsaKey);

            return rsaParameters;
        }
    }
}
