using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Projeto.Data.Repositories
{
    public class ColaboradorRepository : BaseRepository<Colaborador>, IColaboradorRepository
    {
        private readonly DataContext datacontext;

        public ColaboradorRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }

        public bool PasswordExiste(string usuario)
        {
            
                return datacontext.Colaborador
                              .Count(x => x.Usuario == usuario
                                     && x.Senha != null) > 0;
            
        }

        public bool PerfilExiste(string usuario)
        {            
               return datacontext.Colaborador
                              .Count(x => x.Usuario == usuario 
                                     && x.IdPerfil != null) > 0;           
        }

        public Colaborador Consultar(string usuario, string password)
        {
            return datacontext.Colaborador    
                              .Include(x => x.Perfil)
                              .FirstOrDefault(x => x.Usuario == usuario
                                              && x.Senha == password);
        }        
    }
}
