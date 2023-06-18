namespace EComm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int pid { get; set; }
        [Required]
        public string pname { get; set; }
        [Required]
        public string pDescription { get; set; }
        [Required]
        public string pCategory { get; set; }
        [Required]
        public int pQuantity { get; set; }
        [Required]
        public string pImage { get; set; }
        [Required]
        public double pPrice { get; set; }
        public double? pDiscount { get; set; }
        public string? pSpecification { get; set; }
    }
}
