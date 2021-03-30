using Deribit.Core.Messages.Exceptions;
using Newtonsoft.Json;

namespace Deribit.Core.Messages.SessionManagement
{
    public class SetHeartbeatMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "public/set_heartbeat"; }

        public int interval { get; set; }
        public void CheckValidity()
        {
            if (this.interval < 10)
            {
                throw new InvalidParameterException(interval.GetType().ToString(), this.interval,
                    nameof(this.interval));
            }
        }
    }

    public class SetHeartbeatResponse : IResponse<string>
    {
        
    }
}