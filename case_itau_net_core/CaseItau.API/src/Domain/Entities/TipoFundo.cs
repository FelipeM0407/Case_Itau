using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseItau.API.src.Domain.Entities
{
    public class TipoFundo
    {
        [Key]
        [Required]
        public int Codigo { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        public ICollection<Fundo> Fundos { get; set; } = new List<Fundo>();
    }
}
