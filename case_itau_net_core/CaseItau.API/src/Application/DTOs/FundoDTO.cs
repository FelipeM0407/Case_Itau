using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseItau.API.src.Application.DTOs
{
    public class FundoDTO
    {
        public string Codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required]
        [ForeignKey("TipoFundo")]
        public int Codigo_Tipo { get; set; }

        [Column(TypeName = "NUMERIC")]
        public decimal? Patrimonio { get; set; }
    }
}