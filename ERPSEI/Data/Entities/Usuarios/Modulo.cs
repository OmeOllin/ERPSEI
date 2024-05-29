namespace ERPSEI.Data.Entities.Usuarios
{
    public class Modulo
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string NombreNormalizado { get; set; } = string.Empty;

        public int Deshabilitado { get; set; }

        public ICollection<AccesoModulo> Accesos { get; set; } = new List<AccesoModulo>();
    }
}
