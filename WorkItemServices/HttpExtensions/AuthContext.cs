using Flurl;

namespace HttpExtensions
{
    public class AuthContext
    {
        public Url Url { get; }

        public string Token { get; }

        public string Scheme { get; }

        internal AuthContext(Url url, string token, string scheme)
        {
            Url = url;
            Token = token;
            Scheme = scheme;
        }
    }
}
