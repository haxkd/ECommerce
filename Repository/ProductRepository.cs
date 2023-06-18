using EComm.Interface;
using EComm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EComm.Repository
{
    public class ProductRepository : IProducts
    {
        
        readonly DatabaseContext _dbContext;

        public ProductRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Product> GetProducts()
        {
            try
            {
                return _dbContext.Products.ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<Product> ProductsByCat(string cat)
        {
            try
            {
                return _dbContext.Products.Where(x => x.pCategory == cat).ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<Product> ProductsSearch(string query)
        {
            try
            {
                return _dbContext.Products.Where(x => x.pname.Contains(query) || x.pDescription.Contains(query)).ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<string> AllCategories()
        {
            try
            {
                return _dbContext.Products.Select(t => t.pCategory).Distinct().ToList();
            }
            catch
            {
                throw;
            }
        }

        public Product GetProduct(int id)
        {
            try
            {
                Product? product = _dbContext.Products.Find(id);
                if (product != null)
                {
                    return product;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public Product AddProduct(ProductModel product1)
        {
            try
            {
                if (!IsPhoto(product1.pImage.FileName))
                {
                    throw new Exception("Image should be JPG or PNG");
                }

                string path = "";

                if (product1.pImage.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/images"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, product1.pImage.FileName), FileMode.Create))
                    {
                        product1.pImage.CopyToAsync(fileStream);
                    }
                }

                Product product = new Product();
                product.pname = product1.pname;
                product.pDescription = product1.pDescription;
                product.pCategory = product1.pCategory;
                product.pQuantity = product1.pQuantity;
                product.pImage = "images/" + product1.pImage.FileName;
                product.pPrice = product1.pPrice;
                product.pDiscount = product1.pDiscount;
                product.pSpecification = product1.pSpecification;

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                return product;
            }
            catch
            {
                throw;
            }
        }
        public Product UpdateProduct(int id,ProductModel product1)
        {
            try
            {
                var product = _dbContext.Products.FirstOrDefault(x => x.pid == id);
                product.pname = product1.pname;
                product.pDescription = product1.pDescription;
                product.pCategory = product1.pCategory;
                product.pQuantity = product1.pQuantity;
                product.pPrice = product1.pPrice;
                product.pDiscount = product1.pDiscount;
                product.pSpecification = product1.pSpecification;
                if (product1.pImage != null)
                {
                    if (!IsPhoto(product1.pImage.FileName))
                    {
                        throw new Exception("Image should be JPG or PNG");
                    }
                    string path = "";

                    if (product1.pImage.Length > 0)
                    {
                        path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/images"));
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var fileStream = new FileStream(Path.Combine(path, product1.pImage.FileName), FileMode.Create))
                        {
                            product1.pImage.CopyToAsync(fileStream);
                        }
                        product.pImage = "images/" + product1.pImage.FileName;
                    }
                }
                //_dbContext.Entry(product).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return product;
            }
            catch
            {
                throw;
            }
        }
        public Product DeleteProduct(int id)
        {
            try
            {
                Product? product = _dbContext.Products.Find(id);

                if (product != null)
                {
                    _dbContext.Products.Remove(product);
                    _dbContext.SaveChanges();
                    return product;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckProduct(int id)
        {
            return _dbContext.Products.Any(e => e.pid == id);
        }

        public static bool IsPhoto(string fileName)
        {
             List<string> list = new List<string> { ".jpg", ".jpeg", ".png" };

        var filename = fileName.ToLower();
            bool isThere = false;
            foreach (var item in list)
            {
                if (filename.EndsWith(item))
                {
                    isThere = true;
                    break;
                }
            }
            return isThere;
        }
    }
}
