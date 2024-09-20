using ERPSEI.Data.Entities.Clientes;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class Conciliacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal? Total { get; set; }
        public int BancoId { get; set; }
        public Banco? Banco { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
        public int UsuarioCreadorId { get; set; }
        public AppUser? AppUserC { get; set; }
        public int UsuarioModificadorId { get; set; }
        public AppUser? AppUserM { get; set; }
        public ICollection<ConciliacionDetalle>? DetallesConciliacion { get; set; } = [];
        public bool Deshabilitado { get; set; }
    }
}
