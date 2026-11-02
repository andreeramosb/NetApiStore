using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> SaveProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                    nameof(GetProducts),
                    new {id = product.Id},
                    product
                );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateById(Product product, int id)
        {
            //var findProduct = await _dbContext.Products.FindAsync(id);

            if (id != product.Id)
                return BadRequest();

            _dbContext.Entry(product).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);


            if (product == null)
                return NotFound();

            _dbContext.Products.Remove(product);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
