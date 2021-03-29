namespace Deribit.Core.Messages.ResponseObjects
{
    public class Stats
    {
        public float high { get; set; }
        public float low { get; set; }
        public float? price_change { get; set; }
        public float volume { get; set; }
    }
}