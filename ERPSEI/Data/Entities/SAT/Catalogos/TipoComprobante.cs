﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT.Catalogos
{
    public class TipoComprobante
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal ValorMaximo { get; set; }

        public int Deshabilitado { get; set; } = 0;

        public ICollection<Prefactura> Prefacturas { get; set; } = new List<Prefactura>();
    }
}