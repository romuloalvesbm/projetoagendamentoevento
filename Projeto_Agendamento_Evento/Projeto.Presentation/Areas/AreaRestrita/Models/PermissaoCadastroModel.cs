using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class PermissaoCadastroModel
    {
        [Required(ErrorMessage = "Por favor, informe a perfil.")]
        public int? IdPerfil { get; set; }
                
        public List<SelectListItem> Perfis { get; set; }
    }
}
