using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class ConciliacionDetalleMovimiento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ConciliacionDetalleId { get; set; }
        public ConciliacionDetalle? ConciliacionDetalle { get; set; }
        public int? MovimientoBancarioId { get; set; }
        public MovimientoBancario? MovimientoBancario { get; set; }
    }
}
