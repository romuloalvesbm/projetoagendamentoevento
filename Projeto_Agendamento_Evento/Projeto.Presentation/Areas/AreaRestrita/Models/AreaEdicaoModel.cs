using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class AreaEdicaoModel
    {
        public int IdArea { get; set; }
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da area.")]
        public string Nome { get; set; }
        public bool Desativar { get; set; }
    }
}
