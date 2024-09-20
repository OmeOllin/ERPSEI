using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class MovimientoBancario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Importe { get; set; }
        public bool Conciliado { get; set; }
        public Conciliacion? Conciliacion { get; set; }
        public ConciliacionDetalleMovimiento? ConciliacionDetalleMovimiento { get; set; }
    }
}
