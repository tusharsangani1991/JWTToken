
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Tabels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {
        private readonly DbContextEx _context;
        //private readonly ICacheService _cacheService;

        public ProductController(DbContextEx context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ProductsList")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> Get()
        {
            var productCache = new List<ProductTable>();

            //  productCache = _cacheService.GetData<List<Product>>("Product");

            var product = await _context.Products.ToListAsync();
            if (product.Count > 0)
            {
                productCache = product;
                //var expirationTime = DateTimeOffset.Now.AddMinutes(3.0);
                //_cacheService.SetData("Product", productCache, expirationTime);
            }

            return productCache;
        }


        [HttpGet]
        [Route("ProductDetail")]
        public async Task<ActionResult<ProductTable>> Get(int id)
        {
            var productCache = new ProductTable();
            var productCacheList = new List<ProductTable>();

            //  productCacheList = _cacheService.GetData<List<Product>>("Product");

            productCache = productCacheList.Find(x => x.Id == id);

            if (productCache == null)
            {
                productCache = await _context.Products.FindAsync(id);
            }

            return productCache;
        }


        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<ProductTable>> POST(ProductTable product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            // _cacheService.RemoveData("Product");

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);

            // _cacheService.RemoveData("Product");

            await _context.SaveChangesAsync();

            return await _context.Products.ToListAsync();
        }


        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> Update(int id, ProductTable product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var productData = await _context.Products.FindAsync(id);
            if (productData == null)
            {
                return NotFound();
            }

            productData.ProductCost = product.ProductCost;
            productData.ProductDescription = product.ProductDescription;
            productData.ProductName = product.ProductName;
            productData.ProductStock = product.ProductStock;

            // _cacheService.RemoveData("Product");

            await _context.SaveChangesAsync();
            return await _context.Products.ToListAsync();
        }
    }
}
