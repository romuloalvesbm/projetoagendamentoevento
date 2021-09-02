using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class SalaCadastroModel
    {
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da sala.")]
        public string Nome { get; set; }

        public bool Desativar { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da localidade.")]
        public int? IdLocalidade { get; set; }

        public string NomeLocalidade { get; set; }       

        public List<SelectListItem> Localidades { get; set; }
    }
}
