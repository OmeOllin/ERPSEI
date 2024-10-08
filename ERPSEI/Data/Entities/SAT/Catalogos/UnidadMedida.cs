﻿using System.ComponentModel.DataAnnotations.Schema;
using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Entities.SAT.Catalogos
{
    public class UnidadMedida
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string Nota { get; set; } = string.Empty;

        public string Simbolo { get; set; } = string.Empty;

        public int Deshabilitado { get; set; } = 0;

        public ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();

    }
}
