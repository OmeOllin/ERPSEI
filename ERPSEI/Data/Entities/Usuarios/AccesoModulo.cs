namespace ERPSEI.Data.Entities.Usuarios
{
    public class AccesoModulo
    {
        public int Id { get; set; }

        public int PuedeConsultar { get; set; }

        public int PuedeEditar { get; set; }

        public int PuedeEliminar { get; set; }

        public int UsuarioId { get; set; }
        public AppUser? Usuario { get; set; }

        public int ModuloId { get; set; }
        public Modulo? Modulo { get; set; }
    }
}
