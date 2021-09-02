using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class AreaConsultaModel
    {
        public int IdArea { get; set; }

        public List<Area> Areas { get; set; }
        public List<PerfilPermissao> PerfisPermissoes { get; set; }
    }
}
