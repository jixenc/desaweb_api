using DesaWeb.Api.Venta.Models;

namespace DesaWeb.Api.Venta.Services
{
    public interface ICustomerService
    {
        void SaveCustomer(Customer customer);

        void EditCustomer(Customer cusedit, int cuseditId);
    }
}
