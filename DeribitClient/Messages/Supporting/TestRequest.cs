using System.Text.Json.Serialization;

namespace DeribitClient.Messages.Supporting
{
    [MethodName("/public/test")]
    public class TestRequest : IRequest
    {
        public string? expected_result { get; set; }
    }
}