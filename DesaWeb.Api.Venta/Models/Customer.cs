using System.ComponentModel.DataAnnotations;

namespace DesaWeb.Api.Venta.Models
{
    public class Customer
    {
        private string customerCode;
        private string customerFirstName;
        private string customerLastName;
        private string customerNIT;
        private string customerAddress;
        private string customerCategory;
        private string customerStatus;

        public Customer(string customerCode, string customerFirstName, string customerLastName, string customerNIT, string customerAddress, string customerCategory, string customerStatus)
        {
            this.customerCode = customerCode;
            this.customerFirstName = customerFirstName;
            this.customerLastName = customerLastName;
            this.customerNIT = customerNIT;
            this.customerAddress = customerAddress;
            this.customerCategory = customerCategory;
            this.customerStatus = customerStatus;
        }
        [Required(ErrorMessage = "El código de cliente es obligatorio.")]
        public string CustomerCode { get => customerCode; set => customerCode = value; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string CustomerFirstName { get => customerFirstName; set => customerFirstName = value; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string CustomerLastName { get => customerLastName; set => customerLastName = value; }

        [Required(ErrorMessage = "El NIT es obligatorio.")]
        [RegularExpression(@"^\d{4}-\d{6}-\d{3}-\d{1}$", ErrorMessage = "El formato del NIT es incorrecto.")]
        public string CustomerNIT { get => customerNIT; set => customerNIT = value; }
        public string CustomerAddress { get => customerAddress; set => customerAddress = value; }
        public string CustomerCategory { get => customerCategory; set => customerCategory = value; }
        public string CustomerStatus { get => customerStatus; set => customerStatus = value; }


    }
}
