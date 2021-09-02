using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class AreaRepository : BaseRepository<Area>, IAreaRepository
    {
        private readonly DataContext datacontext;

        public AreaRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }

        public int? ObterIdPorNome(string nome)
        {
            return datacontext.Area
                             .Where(x => x.Nome == nome)
                             .Select(x => (int?)x.IdArea)
                             .FirstOrDefault();

        }        

        public List<Area> ConsultarAtivoPorNome(string nome)
        {
            return datacontext.Area
                              .Where(x => x.Nome.StartsWith(nome) && x.Desativar == null)
                              .ToList();
        }

        public bool AreaAtiva(int idarea)
        {
            return datacontext.Area
                              .Count(x => x.IdArea == idarea &&
                                          x.Desativar == null) > 0;
        }
    }
}
