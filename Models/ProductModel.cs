namespace EComm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductModel
    {
       
        public string pname { get; set; }
        public string pDescription { get; set; }
        public string pCategory { get; set; }
        public int pQuantity { get; set; }
        public IFormFile? pImage { get; set; }
        public double pPrice { get; set; }
        public double? pDiscount { get; set; }
        public string? pSpecification { get; set; }
    }
}
