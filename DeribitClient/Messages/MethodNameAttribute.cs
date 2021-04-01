namespace DeribitClient.Messages
{
    public class MethodName : System.Attribute
    {
        public string methodName;

        public MethodName(string methodName)
        {
            this.methodName = methodName;
        }
    }
}