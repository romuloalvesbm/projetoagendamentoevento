using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class PermissaoMenu
    {
        public PermissaoMenu()
        {
            PermissaoSubMenu = new List<PermissaoMenu>();
        }
        
        public int IdPermissao { get; set; }
        public string Descricao { get; set; }
        public bool Check { get; set; }
        public int? IdPai { get; set; }

        public List<PermissaoMenu> PermissaoSubMenu { get; set; }
    }
}
