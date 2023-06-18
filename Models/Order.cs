using System.ComponentModel.DataAnnotations;

namespace EComm.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int PId { get; set; }
        public int quantity { get; set; }
        public string OrderdId { get; set; }
        public string date { get; set; }
        public int uid { get; set; }
       
    }
}
