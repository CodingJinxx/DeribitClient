namespace Deribit.Core.Types
{
    public class AdvancedOrderType
    {
        public const string USD = "usd";
        public const string ImpliedVolatility = "implv";

        public AdvancedOrderType()
        {
            
        }

        public static bool Contains(string input)
        {
            if (input == USD || input == ImpliedVolatility)
            {
                return true;
            }
            return false;
        }
    }
}