﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT
{
    public class Moneda
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

		public int Decimales { get; set; }

		public int PorcentajeVariacion { get; set; }

	}
}