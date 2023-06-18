namespace EComm.Models
{
    public class CartModel
    {
        public int cid { get; set; }
        public int pid { get; set; }
        public Product? product { get; set; }
        public int uid { get; set; }
        public int quantity { get; set; }
    }
}
