using ERPSEI.Data.Entities.SAT.cfdiv40;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class ConciliacionDetalle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ConciliacionId { get; set; }
        //public Conciliacion? Conciliacion { get; set; }
        public int? ComprobanteId { get; set; }
        //public Comprobante? Comprobante { get; set; }
        public int? MovimientoId { get; set; }
        //public MovimientoBancario? MovimientoBancario { get; set; }
    }
}
