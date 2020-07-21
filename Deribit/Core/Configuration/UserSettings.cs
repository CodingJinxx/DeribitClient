namespace Deribit.Core.Configuration
{
    public class UserSettings
    {
        public const string SectionName = nameof(UserSettings);
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
    }
}