namespace DeribitClient.DeribitTypes
{
    public class Stats
    {
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal? price_change { get; set; }
        public decimal volume { get; set; }
    }
}