using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT.Catalogos
{
    public class TipoFactor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public ICollection<TasaOCuota>? TasasOCuotas { get; set; }

        public int Deshabilitado { get; set; } = 0;

    }
}
