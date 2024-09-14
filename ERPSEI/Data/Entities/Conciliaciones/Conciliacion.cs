using ERPSEI.Data.Entities.Empleados;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class Conciliacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public TimeSpan? Fecha { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Total { get; set; }
        public int BancoId { get; set; }
        public int ClienteId { get; set; }
        public int EmpresaId { get; set; }
        public int UsuarioCreadorId { get; set; }
        public int UsuarioModificadorId { get; set; }
        public string? DetallesConciliacion { get; set; }
        public bool Deshabilitado { get; set; }
    }
}
