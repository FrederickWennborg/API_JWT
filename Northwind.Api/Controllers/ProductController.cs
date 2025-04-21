using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Api.Contexts;
using Northwind.Api.DTOs;
using Northwind.Api.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly NorthwindContext _dbContext;

        public ProductController(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }


        //Vi ska såklart använda services! 

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<List<ProductReadDTO>>> GetAll()
        {
            var productDtos = await _dbContext.Products
                   .Include(p => p.Category)
                   .Select(productEntity => new ProductReadDTO
                   {
                       ProductName = productEntity.ProductName,
                       CategoryId = Convert.ToInt32(productEntity.CategoryId),
                       SupplierId = Convert.ToInt32(productEntity.SupplierId),
                       QuantityPerUnit = productEntity.QuantityPerUnit,
                       UnitPrice = Convert.ToInt32(productEntity.UnitPrice),
                       UnitsInStock = Convert.ToInt32(productEntity.UnitsInStock),
                       UnitsOnOrder = Convert.ToInt32(productEntity.UnitsOnOrder),
                       ReorderLevel = Convert.ToInt32(productEntity.ReorderLevel),
                       Discontinued = productEntity.Discontinued,
                       CategoryName = productEntity.Category.CategoryName
                   })
                   .ToListAsync();

            if (productDtos == null)
            {
                return NotFound();
            }

            return Ok(productDtos);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDTO>> Get(int id)
        {
            var productEntity = await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            var productDto = new ProductReadDTO
            {
                ProductName = productEntity.ProductName,
                CategoryId = Convert.ToInt32(productEntity.CategoryId),
                SupplierId = Convert.ToInt32(productEntity.SupplierId),
                QuantityPerUnit = productEntity.QuantityPerUnit,
                UnitPrice = Convert.ToInt32(productEntity.UnitPrice),
                UnitsInStock = Convert.ToInt32(productEntity.UnitsInStock),
                UnitsOnOrder = Convert.ToInt32(productEntity.UnitsOnOrder),
                ReorderLevel = Convert.ToInt32(productEntity.ReorderLevel),
                Discontinued = productEntity.Discontinued,
                CategoryName = productEntity.Category.CategoryName
            };

            return Ok(productDto);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult> Post(ProductCreateDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest();
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
                QuantityPerUnit = productDto.QuantityPerUnit,
                UnitPrice = productDto.UnitPrice,
                UnitsInStock = Convert.ToInt16(productDto.UnitsInStock),
                UnitsOnOrder = Convert.ToInt16(productDto.UnitsOnOrder),
                ReorderLevel = Convert.ToInt16(productDto.ReorderLevel),
                Discontinued = productDto.Discontinued
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Created();
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ProductCreateDTO productDto) //ProductUpdateDTO ??? Kanske mer cleant, men vi skippar det här. 
        {
            var productEntity = await _dbContext.Products.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.ProductName = productDto.ProductName;
            productEntity.CategoryId = productDto.CategoryId;
            productEntity.SupplierId = productDto.SupplierId;
            productEntity.QuantityPerUnit = productDto.QuantityPerUnit;
            productEntity.UnitPrice = productDto.UnitPrice;
            productEntity.UnitsInStock = Convert.ToInt16(productDto.UnitsInStock);
            productEntity.UnitsOnOrder = Convert.ToInt16(productDto.UnitsOnOrder);
            productEntity.ReorderLevel = Convert.ToInt16(productDto.ReorderLevel);
            productEntity.Discontinued = productDto.Discontinued;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }


        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var productEntity = await _dbContext.Products.FindAsync(id);
            if (productEntity != null)
            {
                _dbContext.Products.Remove(productEntity);
                _dbContext.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

    }

}
