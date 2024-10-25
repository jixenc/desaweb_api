using DesaWeb.Api.Venta.Models;

namespace DesaWeb.Api.Venta.Services
{
    public interface IProductService
    {
        Task CreateProduct(Product product);

        Task<List<Product>> ConsultProduct(string productCode);

        Task UpdateProduct(string prodCode, Product product);

        Task DownProduct(string prodCode);

    }
}
