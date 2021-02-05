namespace Deribit.Core.Types
{
    public class TimeInForce
    {
        public const string GoodTillCancelled = "good_til_cancelled";
        public const string FillOrKill = "fill_or_kill";
        public const string ImmediateOrCancel = "immediate_or_cancel";

        public TimeInForce()
        {
            
        }

        public static bool Contains(string input)
        {
            if (input == GoodTillCancelled || input == FillOrKill || input == ImmediateOrCancel)
            {
                return true;
            }
            return false;
        }
    }
}