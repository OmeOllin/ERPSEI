using ERPSEI.Data.Entities.SAT.cfdiv40;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class ConciliacionDetalleComprobante
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ConciliacionDetalleId { get; set; }
        public ConciliacionDetalle? ConciliacionDetalle { get; set; }
        public int? ComprobanteId { get; set; }
        public Comprobante? Comprobante { get; set; }
    }
}
