namespace ERPSEI.Data.Entities.Usuarios
{
    public class AccesoModulo
    {
        public int Id { get; set; }

        public int PuedeConsultar { get; set; }

        public int PuedeEditar { get; set; }

        public int PuedeEliminar { get; set; }

        public string? RolId { get; set; }
        public AppRole? Rol { get; set; }

        public int? ModuloId { get; set; }
        public Modulo? Modulo { get; set; }
    }
}
