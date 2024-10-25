namespace DesaWeb.Api.Venta.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }  // ID del usuario
        public string Correo { get; set; }   // Correo del usuario
        public string Contraseña { get; set; }  // Contraseña del usuario
        public string Nombre { get; set; }  // Nombre del usuario
        public string Apellido { get; set; }  // Apellido del usuario
        public string Telefono { get; set; }  // Teléfono del usuario
        public string Direccion { get; set; }  // Dirección del usuario
        public string Estado { get; set; }  // Estado ('V' para activo, 'I' para inactivo)
        public int RolID { get; set; }  // ID del rol del usuario
        public string Accion { get; set; }  // Acción ('C' para crear, 'R' para leer, 'D' para eliminar)
    }


}
