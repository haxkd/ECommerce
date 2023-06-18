using EComm.Models;

namespace EComm.Interface
{
    public interface IProducts
    {
        public List<Product> GetProducts();
        public List<Product> ProductsByCat(string cat);
        public List<Product> ProductsSearch(string query);
        public Product GetProduct(int id);
        public Product AddProduct(ProductModel product);
        public Product UpdateProduct(int id,ProductModel product);
        public Product DeleteProduct(int id);
        public bool CheckProduct(int id);
        public List<string> AllCategories();
    }
}
