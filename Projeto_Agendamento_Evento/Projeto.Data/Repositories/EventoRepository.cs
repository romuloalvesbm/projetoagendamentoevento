using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class EventoRepository : BaseRepository<Evento>, IEventoRepository
    {
        private readonly DataContext datacontext;

        public EventoRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }       

        public int? ObterIdPorNome(string nome)
        {
            return datacontext.Evento
                             .Where(u => u.Nome == nome)                             
                             .Select(x => (int?)x.IdEvento)
                             .FirstOrDefault();
                             
        }       

        public List<Evento> ConsultarAtivoPorNome(string nome)
        {
            return datacontext.Evento
                              .Where(x => x.Nome.StartsWith(nome) && 
                                          x.Desativar == null)
                              .ToList();
        }

        public bool EventoAtivo(int idevento)
        {
            return datacontext.Evento
                              .Count(x => x.IdEvento == idevento &&
                                          x.Desativar == null) > 0;
        }
    }
}
