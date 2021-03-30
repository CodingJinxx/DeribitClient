using System.Text.Json.Serialization;

namespace Deribit.Core.Messages.Supporting
{
    public class TestMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/public/test"; }
        public string? expected_result { get; set; }
        public void CheckValidity()
        {
            // TODO Implement Check Validity
        }
    }

    public class TestMessageResponse : IResponse<TestMessageResponse>
    {
        public string version { get; set; }
    }
}