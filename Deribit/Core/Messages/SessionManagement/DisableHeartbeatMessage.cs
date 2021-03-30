namespace Deribit.Core.Messages.SessionManagement
{
    public class DisableHeartbeatMessage : IMessage
    {
        public string MethodName { get => "/public/disable_heartbeat"; }
        public void CheckValidity()
        {
            
        }
    }
    
    public class DisableHeartbeatResponse : IResponse<string>
    {
        
    }
}