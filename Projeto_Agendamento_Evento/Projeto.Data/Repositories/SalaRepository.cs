using Microsoft.EntityFrameworkCore;
using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class SalaRepository : BaseRepository<Sala>, ISalaRepository
    {
        private readonly DataContext datacontext;

        public SalaRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }

        public int? ObterIdPorCriterio(string nome, int idlocalidade)
        {
            return datacontext.Sala
                             .Where(x => x.Nome == nome &&
                                         x.IdLocalidade == idlocalidade)
                             .Select(x => (int?)x.IdSala)
                             .FirstOrDefault();

        }

        public List<Sala> ConsultarAtivoPorNome(string nome)
        {
            return datacontext.Sala
                              .Include(x => x.Localidade)
                              .Where(x => x.Nome.StartsWith(nome) && x.Desativar == null)
                              .ToList();
        }       

        public List<Sala> ConsultarTodos()
        {
            return datacontext.Sala
                              .Include(x => x.Localidade)                             
                              .ToList();
        }

        public bool SalaAtiva(int idsala)
        {
            return datacontext.Sala
                              .Count(x => x.IdSala == idsala &&
                                          x.Desativar == null) > 0;
        }
    }
}
