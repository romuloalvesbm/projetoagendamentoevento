using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto.Data.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class PermissaoEdicaoModel
    {
        [Required(ErrorMessage = "Por favor, informe o perfil.")]
        public int IdPerfil { get; set; }

        public List<SelectListItem> Perfis { get; set; }
        public List<PermissaoMenu> PermissoesMenu { get; set; }
    }
}
