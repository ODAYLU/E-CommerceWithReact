using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProduct product, IWebHostEnvironment webHostEnvironment)
        {
            _product = product;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult>  GetAll()
        {
            var data = await _product.GetAll();
            return Ok(data);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm]Product data)
        {
            if(!ModelState.IsValid) return BadRequest("The Data Is Not Valid");
            if(data.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + @"\images\";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(data.File.FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await data.File.CopyToAsync(fileStream);
                }
                data.Image ="/images/"+ fileName + extension;

            }
            await _product.AddProduct(data);
            return Ok("the Product Added Successfully");
        }
        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody]Product data)
        {
            if (!ModelState.IsValid) return BadRequest("The Data Is Not Valid");
            var productOld = await _product.GetById(data.Id);
            if (data.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + @"/images/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(data.File.FileName);
                var path = webRootPath + productOld.Image;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await data.File.CopyToAsync(fileStream);
                }
                data.Image = "/images/" + fileName + extension;

            }
            else
            {
                data.Image = productOld.Image;
            }
            await  _product.EditProduct(data);
            return Ok("the Product Updated Successfully");
        }
        [HttpGet("DeleteProduct/{id}")]
        public  async Task<IActionResult>  DeleteProduct(int  id)
        {
            if (!ModelState.IsValid) return BadRequest("The Data Is Not Valid");
          var result =  await _product.DeleteProduct(id);
            return Ok("the Product Added Successfully");
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductId(int id)
        {
            if (id == 0) return BadRequest("Must Enter the Id");
            return Ok(await _product.GetById(id));
            }
    }
}
