using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HCCategory.Models;
using Microsoft.EntityFrameworkCore;

namespace HCCategory.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CContext _dbContext;
        public CategoryController(CContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET All Categories
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        //POST new category
        [HttpPost("{newcategory}")]
        public async Task<ActionResult<Category>> PostCategory(string newcategory)
        {
            if (string.IsNullOrEmpty(newcategory))
            {
                return BadRequest();
            }
            var category = new Category { Name = newcategory };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
        //POST new extra
        [HttpPost("NewExtra")]        
        public async Task<ActionResult<Extra>> CreateProduct(Extra extra)
        {
            var category = await _dbContext.Categories.FindAsync(extra.CategoryId);
            if (category == null)
            {
                return BadRequest("The category does not exist.");
            }

            
            _dbContext.Extras.Add(extra);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = extra.Id }, extra);
        }
        //Delete by Name(и даже extra и продукты)
        [HttpDelete("Categories/{name}")]
        public async Task<ActionResult> DeleteCategoryByName(string name)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Name == name);
      
            if (category == null)
            {
                return NotFound();
            }
            var extras = await _dbContext.Extras.Where(c => category.Id == c.CategoryId).ToListAsync();
            var products = await _dbContext.Products.Where(p => category.Id == p.CategoryId).ToListAsync();
            _dbContext.Extras.RemoveRange(extras);
            _dbContext.Products.RemoveRange(products);
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        //Get all products
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        //Get products by CategoryID
        [HttpGet("ProductsByCategory/{Id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategory(int Id)
        {
            if (_dbContext.Categories == null) { return NotFound(); }
            var category = await _dbContext.Categories.FindAsync(Id);
            if (category == null) { return NotFound(); }
            var tovary = await _dbContext.Products.Where(t => t.CategoryId == category.Id).ToListAsync();
            return tovary;
        }
        // Get products by Category and Extra
        [HttpGet("ProductsByCategoryAndExtra")]
        public async Task<ActionResult<IEnumerable<Product>>> GetTovarsByCategoryAndParams(string category, string name, string value)
        {
            var temp = await _dbContext.Categories.SingleOrDefaultAsync(t => t.Name == category);
            var temp1 = await _dbContext.Extras.SingleOrDefaultAsync(t => t.Name == name && t.Value == value && t.CategoryId == temp.Id);

            var tovary = await _dbContext.Products
                           .Where(t => t.Id == temp1.ProductId)
                           .ToListAsync();
            if (tovary == null)
            {
                return NotFound();
            }
            return tovary;
        }
        //GET product by ID
        [HttpGet("Products/{Id}")]
        public async Task<ActionResult<Product>> GetProduct(int Id)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(t => t.Id == Id);
            if (product == null) { return NotFound(); }
            return product;
        }

        //Post new product
        [HttpPost("NewProduct")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var category = await _dbContext.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("The category does not exist.");
            }

            //extra.CategoryId = category.Id;
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }
}
