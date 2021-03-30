namespace Deribit.Core.Messages.ResponseObjects
{
    public class Greeks
    {
        public decimal delta { get; set; }
        public decimal gamma { get; set; }
        public decimal rho { get; set; }
        public decimal theta { get; set; }
        public decimal vega { get; set; }
    }
}