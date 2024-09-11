namespace DesaWeb.Api.Venta.Models
{
    public class User
    {
        public int UserId { get; set; }  // ID del usuario
        public string UserEmail { get; set; }   // Correo del usuario
        public string UserPass { get; set; }  // Contraseña del usuario
        public string UserUserName { get; set; }  // Nombre del usuario
        public string UserName { get; set; }  // Apellido del usuario
        public string UserLastName { get; set; }  // Teléfono del usuario
        public string UserAddress { get; set; }  // Dirección del usuario
        public string UserBlock { get; set; }  // Estado ('V' para activo, 'I' para inactivo)
        public int UserRolId { get; set; }  // ID del rol del usuario
        public string UserAction { get; set; }  // Acción ('C' para crear, 'R' para leer, 'D' para eliminar)
    }
}
