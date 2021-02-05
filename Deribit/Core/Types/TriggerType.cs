namespace Deribit.Core.Types
{
    public class TriggerType
    {
        public const string IndexPrice = "index_price";
        public const string MarkPrice = "mark_price";
        public const string LastPrice = "last_price";

        public TriggerType()
        {
            
        }

        public static bool Contains(string input)
        {
            if (input == IndexPrice || input == MarkPrice || input == LastPrice)
            {
                return true;
            }

            return false;
        }
    }
}