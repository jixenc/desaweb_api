using DesaWeb.Api.Venta.Models;
using DesaWeb.Api.Venta.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace DesaWeb.Api.Venta.Controllers.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("ProductCreate")]
        public async Task<IActionResult> CrearProducto([FromBody] Product producto)
        {

            if (producto == null)
            {
                return BadRequest("El producto es inválido.");
            }

            try
            {

                await _productService.CreateProduct(producto);


                return Ok("Producto creado exitosamente.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al crear el producto: {ex.Message}");
            }
        }


        [HttpGet("ProductConsult/ProductCode/{pcode}")]
        public async Task<IActionResult> ConsultProduct(string productquery)
        {
            if (string.IsNullOrEmpty(productquery))
            {
                return BadRequest("Parametrización inválida en la búsqueda del producto.");
            }

            try
            {

                var producto = await _productService.ConsultProduct(productquery);


                if (producto == null)
                {
                    return NotFound($"No se encontró un producto con la busqueda : {productquery}.");
                }


                return Ok(producto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al consultar el producto: {ex.Message}");
            }
        }


        [HttpPut("ProductUpdate/{pcode}")]
        public async Task<IActionResult> ActualizaProducto(string pcode, Product productoUpdate)
        {
            if (string.IsNullOrEmpty(pcode) || productoUpdate == null)
            {
                return BadRequest("Parametrización inválida o datos del producto no proporcionados.");
            }

            try
            {

                var productoExistente = await _productService.ConsultProduct(pcode);
                if (productoExistente == null)
                {
                    return NotFound($"No se encontró un producto con el código: {pcode}.");
                }


                await _productService.UpdateProduct(pcode, productoUpdate);

                return Ok($"Producto con código {pcode} actualizado exitosamente.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al actualizar el producto: {ex.Message}");
            }

        }


        [HttpDelete("ProductDown/{pcode}")]
        public async Task<IActionResult> BajaProducto(string pcode)
        {

           
            if (string.IsNullOrEmpty(pcode))
            {
                return BadRequest("Parametrización inválida en la eliminación del producto.");
            }

            try
            {
              
                var productoExistente = await _productService.ConsultProduct(pcode);
                if (productoExistente == null)
                {
                    return NotFound($"No se encontró un producto con el código: {pcode}.");
                }


                await _productService.DownProduct(pcode);

                return Ok($"Producto con código {pcode} eliminado exitosamente.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
