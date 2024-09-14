using DesaWeb.Api.Venta.Models;

namespace DesaWeb.Api.Venta.Services
{
    public class ProductService : IProductService
    {
        public Task<List<Product>> ConsultProduct(string productCode)
        {
            throw new NotImplementedException();
        }

        public Task CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DownProduct(string prodCode)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(string prodCode, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
