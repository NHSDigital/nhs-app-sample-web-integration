using Org.BouncyCastle.OpenSsl;

namespace nhsapp.sample.web.integration.Certificate
{
    internal class PasswordFinder : IPasswordFinder
    {
        private readonly string _password;

        public PasswordFinder(string password)
        {
            _password = password;
        }

        public char[] GetPassword()
        {
            return _password.ToCharArray();
        }
    }
}