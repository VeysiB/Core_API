using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]   
    [Route("api/[Controller]")]
    public class ProductsController:ControllerBase
    {
        private readonly IProductRepository _productRepository;

        //Ok(200),NotFound(404),NoContent(204),Created(201),BadRequest(400)
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result= await _productRepository.GetAllAsync();

            return Ok(result);
        }  
        [HttpGet("{id}")]
        //api/products/1 FromRoute  [HttpGet("{id}")]
        //api/products?id=1 FromQuery    [HttpGet("{getById}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data= await _productRepository.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound(id);
            }
            return Ok(data);
        }
        [HttpPost]
        //bir nesne aldığı için default olarak FromBody dir
        public async Task<IActionResult> Create(Product product)
        {
            var addedProduct = await _productRepository.CreateAsync(product);
            return Created(string.Empty, addedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            var checkProduct = await _productRepository.GetByIdAsync(product.Id);
            if (checkProduct == null)
            {
                return NotFound(product.Id);
            }
            await _productRepository.UpdateAsync(product);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkProduct = await _productRepository.GetByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _productRepository.RemoveAsync(id);
            return NoContent();
        }
        //api/products/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            var newName= Guid.NewGuid() + "." + Path.GetExtension(formFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
            var stream = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return Created(string.Empty, formFile);
        }
        /*[FromQuery] api/products?id=1&name= &telefon&*/

        #region FromForm ve FromHeader kullanım örneği
        //[HttpGet("[action]")]
        //public IActionResult Test([FromForm] string name,[FromHeader] string auth)
        //{
        //    var authentication = HttpContext.Request.Headers["auth"];
        //    var name2 = HttpContext.Request.Form["name"];
        //    return Ok();
        //}
        #endregion
        [HttpGet("[action]")]
        public IActionResult Test([FromServices] IDummyRepository dummyRepository)
        {
            return Ok(dummyRepository.GetName());
        }
    }
}
