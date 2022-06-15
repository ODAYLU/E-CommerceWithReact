using E_Commerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public double Price { get; set; }
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Image { get; set; } = "";
        public double Rate { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
     
    public interface IProduct
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<Product> GetByCat(string Cat);
        Task<List<Product>> DeleteProduct(int id);
        Task<bool> AddProduct(Product product);
        Task<bool> EditProduct(Product product);

    }

    public class ManageProduct : IProduct
    {
        private readonly ApplicationDbContext _context;

        public ManageProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AddProduct(Product product)
        {
            if (product is not null)
            {
              await  _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Product>> DeleteProduct(int id)
        {
            if (id != 0)
            {
                try
                {
                    var data = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
                    if(data is not null)
                    {
                        _context.Products.Remove(data);
                        await _context.SaveChangesAsync();
                        return await GetAll();
                    }
                    return new List<Product>();
                }catch(Exception ex)
                {
                    throw;
                    
                }
                
               
            }
            return new List<Product>();
        }

        public async Task<bool> EditProduct(Product product)
        {
            if (product is not null)
            {
                 _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Product>> GetAll()
        {
            var data = await _context.Products.ToListAsync();
            return data;
        }

        public async Task<Product> GetByCat(string Cat)
        {
            
            var data = await _context.Products.Where(x => x.Category == Cat).FirstOrDefaultAsync();
            return data;
        }

        public async Task<Product> GetById(int id)
        {
            var data = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }
    }
}
