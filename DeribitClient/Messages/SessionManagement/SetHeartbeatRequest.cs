using Newtonsoft.Json;

namespace DeribitClient.Messages.SessionManagement
{
    [MethodName("public/set_heartbeat")]
    public class SetHeartbeatRequest : IRequest
    {
        public int interval { get; set; }
    }
}