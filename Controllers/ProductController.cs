using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _proRepository;
        public ProductController(IProductRepository proRepository)
        {
            _proRepository = proRepository;
        }

        [HttpGet("{name}", Name = "Search products by name./ Пребарај производ по име.")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchByName(string name)
        {
            try
            {
                var result = await _proRepository.SearchByName(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound("No product with that name./ Нема производ со тоа име.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet(Name = "Print all products./ Печати ги сите производи.")]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                return Ok(await _proRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet("{id:int}", Name = "Search product by id./ Пребарај производ по идентификационен број.")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var result = await _proRepository.Get(id);

                if (result == null)
                {
                    return NotFound("Product id does not exist./ Производ со овој идентификационен број не постои.");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpPost(Name = "Enter new product./ Внеси нов производ.")]
        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest();

                var pro = await _proRepository.Get(product.Id);

                if (pro != null)
                {
                    ModelState.AddModelError("ID", "Product already in use./ Веќе имаме го имаме овој производ.");
                    return BadRequest(ModelState);
                }

                var createdProduct = await _proRepository.Create(product);

                return CreatedAtAction(nameof(GetProduct),
                    new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new product record./ Грешка при креирање нов производ.");
            }
        }

        [HttpPut("{id:int}", Name = "Update product./ Измени производ.")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("Product ID mismatch./ Неусогласеност помеѓу индентификациониот број и производот.");

                var productToUpdate = await _proRepository.Get(id);

                if (productToUpdate == null)
                {
                    return NotFound($"Product with Id = {id} not found./ Производ со број = {id} не беше пронајден");
                }

                return await _proRepository.Update(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating product record./ Грешка при ажурирање на производот");
            }
        }

        [HttpDelete("{id:int}", Name = "Delete product./ Избриши производ.")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var productToDelete = await _proRepository.Get(id);

                if (productToDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found./ Производ со број = {id} не беше пронајден");
                }

                await _proRepository.Delete(id);

                return Ok($"Product with Id = {id} deleted./ Производ со број = {id} е избришан");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting product record. No product was deleted./ Грешка при бришење на производ. Ниту еден производ не беше избришан");
            }
        }
    }
}
