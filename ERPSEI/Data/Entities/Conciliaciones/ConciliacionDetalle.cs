using ERPSEI.Data.Entities.SAT.cfdiv40;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class ConciliacionDetalle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ConciliacionId { get; set; }
        public Conciliacion? Conciliacion { get; set; }
        public ICollection<ConciliacionDetalleComprobante>? ConciliacionesDetallesComprobantes { get; set; } = [];
        public ICollection<ConciliacionDetalleMovimiento>? ConciliacionesDetallesMovimientos { get; set; } = [];
    }
}
