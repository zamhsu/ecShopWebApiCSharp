using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Dtos.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcShopContext _context;

        public ProductController(EcShopContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(BaseRequest<CreateProductDTO> baseRequest)
        {
            var startOffset = new DateTimeOffset(baseRequest.Data.StartDisplay, baseRequest.UserTimeZone);
            var endOffset = new DateTimeOffset(baseRequest.Data.EndDisplay, baseRequest.UserTimeZone);

            Product product = new Product()
            {
                Guid = Guid.NewGuid().ToString(),
                Title = baseRequest.Data.Title,
                CategoryId = baseRequest.Data.CategoryId,
                UnitId = baseRequest.Data.UnitId,
                Quantity = baseRequest.Data.Quantity,
                OriginPrice = baseRequest.Data.OriginPrice,
                Price = baseRequest.Data.Price,
                Description = baseRequest.Data.Description,
                StartDisplay = startOffset.ToUniversalTime(),
                EndDisplay = endOffset.ToUniversalTime(),
                ImageUrl = baseRequest.Data.ImageUrl,
                Memo = baseRequest.Data.Memo,
                StatusId = baseRequest.Data.StatusId,
                CreateDate = new DateTimeOffset(DateTime.UtcNow).ToUniversalTime()
            };

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
