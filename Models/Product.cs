
namespace Models
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public string comment { get; set; }
        public int quantity { get; set; }
        public double paidPrice { get; set; }
        public double unitPrice { get; set; }
        public string remoteCode { get; set; }
        public string description { get; set; }
        public string vatPercentage { get; set; }
        public float discountAmount { get; set; }
    }
}