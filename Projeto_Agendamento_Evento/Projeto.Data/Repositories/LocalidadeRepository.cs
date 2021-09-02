using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class LocalidadeRepository : BaseRepository<Localidade>, ILocalidadeRepository
    {
        private readonly DataContext datacontext;

        public LocalidadeRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }

        public int? ObterIdPorNome(string nome)
        {
            return datacontext.Localidade
                             .Where(x => x.Nome == nome)
                             .Select(x => (int?)x.IdLocalidade)
                             .FirstOrDefault();

        }       

        public List<Localidade> ConsultarAtivoPorNome(string nome)
        {
            return datacontext.Localidade
                              .Where(x => x.Nome.StartsWith(nome) && x.Desativar == null)
                              .ToList();
        }

        public List<Localidade> ConsultarAtivo()
        {
            return datacontext.Localidade
                             .Where(x => x.Desativar == null)
                             .ToList();
        }
    }
}
