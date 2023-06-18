using System.ComponentModel.DataAnnotations;

namespace EComm.Models
{
    public class Cart
    {
        [Key]
        public int cid { get; set; }
        public int pid { get; set; }
        public int uid { get; set; }
        public int quantity { get; set; }
    }
}
