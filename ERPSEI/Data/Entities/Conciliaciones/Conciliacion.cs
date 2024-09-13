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
        public string? Cliente { get; set; }
        public decimal? Total { get; set; }
        public string? UsuarioCreador { get; set; }
        public string? UltimoUsuarioModificador { get; set; }

    }
}
