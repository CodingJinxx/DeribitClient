namespace Deribit.Core.Types
{
    public class GrantType
    {
        public const string ClientCredentials = "client_credentials";
        public const string ClientSignature = "client_signature";
        public const string RefreshToken = "refresh_token";

        private GrantType()
        {

        }

        public static bool Contains(string input)
        {
            if(input == ClientCredentials || input == ClientSignature || input == RefreshToken)
            {
                return true;
            }
            return false;
        }
    }
}
