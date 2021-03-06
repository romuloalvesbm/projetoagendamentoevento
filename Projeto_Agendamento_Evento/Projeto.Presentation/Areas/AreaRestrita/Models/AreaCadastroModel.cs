using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class AreaCadastroModel
    {
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do area.")]
        public string Nome { get; set; }       
        public bool Desativar { get; set; }       
    }
}
