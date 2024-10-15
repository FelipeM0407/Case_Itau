using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CaseItau.API.src.Domain.Entities
{
    public class Fundo
    {
        [Key]
        [Required]
        [StringLength(20)]
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

        [JsonIgnore]
        public TipoFundo TipoFundo { get; set; }
    }
}
