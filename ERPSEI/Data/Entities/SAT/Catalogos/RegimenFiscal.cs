﻿using ERPSEI.Data.Entities.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSEI.Data.Entities.SAT.Catalogos
{
    public class RegimenFiscal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public bool AplicaPersonaFisica { get; set; }

        public bool AplicaPersonaMoral { get; set; }

        public int Deshabilitado { get; set; } = 0;

        public ICollection<Empresa>? Empresas { get; }

    }
}