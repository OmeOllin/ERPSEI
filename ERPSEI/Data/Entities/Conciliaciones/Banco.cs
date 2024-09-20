using System.ComponentModel.DataAnnotations.Schema;
namespace ERPSEI.Data.Entities.Conciliaciones
{
    public class Banco
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Deshabilitado { get; set; }
        public ICollection<Conciliacion>? Conciliaciones { get; set; } = [];
    }
}
