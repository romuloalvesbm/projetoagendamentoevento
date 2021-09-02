using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class EventoConsultaModel
    {
        public int IdEvento { get; set; }        

        public List<Evento> Eventos { get; set; }

        public List<PerfilPermissao> PerfisPermissoes { get; set; }
    }
}
