namespace Deribit.Core.Types
{
    public class OrderType
    {
        public const string Limit = "limit";
        public const string StopLimit = "stop_limit";
        public const string Market = "market";
        public const string StopMarket = "stop_market";

        private OrderType()
        {

        }

        public static bool Contains(string input)
        {
            if(input == Limit || input == StopLimit || input == Market || input == StopMarket)
            {
                return true;
            }
            return false;
        }
    }
}