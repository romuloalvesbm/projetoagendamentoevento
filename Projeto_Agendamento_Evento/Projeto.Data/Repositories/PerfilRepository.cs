using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class PerfilRepository : BaseRepository<Perfil>, IPerfilRepository
    {
        private readonly DataContext datacontext;

        public PerfilRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }
    }
}
