using Microsoft.EntityFrameworkCore;
using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class PermissaoRepository : BaseRepository<Permissao>, IPermissaoRepository
    {
        private readonly DataContext datacontext;

        public PermissaoRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }

        public List<Permissao> ObterPai()
        {
            return datacontext.Permissao   
                              .Include(x => x.Permissoes)
                              .Where(x => x.IdPai == null)
                              .OrderBy(x => x.IdPermissao)
                              .ToList();
        }
    }
}
