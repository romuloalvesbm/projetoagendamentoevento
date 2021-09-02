using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class SalaConsultaModel
    {
        public int IdSala { get; set; }

        public List<Sala> Salas { get; set; }

        public List<PerfilPermissao> PerfisPermissoes { get; set; }
    }
}
