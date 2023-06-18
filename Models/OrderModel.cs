namespace EComm.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int PId { get; set; }
        public int quantity { get; set; }
        public string OrderdId { get; set; }
        public string date { get; set; }
        public int uid { get; set; }
        public Product? product { get; set; }
    }
}
