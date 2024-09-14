using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class ConciliacionDetalle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ConciliacionId { get; set; }
        public int ComprobanteId { get; set; }
        public int MovimientoId { get; set; }
    }
}
