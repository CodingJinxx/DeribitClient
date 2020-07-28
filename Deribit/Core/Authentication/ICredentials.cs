namespace Deribit.Core.Authentication
{
    public interface ICredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string Signature { get; set; }
    }
}
