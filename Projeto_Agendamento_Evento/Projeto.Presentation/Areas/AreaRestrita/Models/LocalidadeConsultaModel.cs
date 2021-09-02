using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class LocalidadeConsultaModel
    {
        public int IdLocalidade { get; set; }

        public List<Localidade> Localidades { get; set; }

        public List<PerfilPermissao> PerfisPermissoes { get; set; }
    }
}
