namespace Rincon.Models
{
    public class CardStock
    {
        public string Id { get; set; }
        public int StockAvailable { get; set; }
        public string Description { get; set; }
        public string ProductDescription { get; set; }
        public string ProductLocation { get; set; }
        public string ProductType { get; set; }
        public string MachimbreDeckLabel { get; set; }
        public string MachimbreDeckSubLabel { get; set; }
        public string WoodState { get; set; }
        public int StockReserved { get; set; }
        public int StockProcess { get; set; }
        public int StockTotal { get; set; }
        public string Icon { get; set; }

    }
}
