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
    public class PerfilPermissaoRepository : BaseRepository<PerfilPermissao>, IPerfilPermissaoRepository
    {
        private readonly DataContext datacontext;

        public PerfilPermissaoRepository(DataContext datacontext) : base(datacontext)
        {
            this.datacontext = datacontext;
        }       

        public bool PermissaoAutorizada(string perfil, int idpermissao)
        {
            return datacontext.PerfilPermissao
                             .Count(x => x.Perfil.Descricao == perfil &&
                                         x.IdPermissao == idpermissao) > 0;
        }

        public PerfilPermissao PermissaoAutorizada(int idperfil, int idpermissao)
        {
            return datacontext.PerfilPermissao
                             .Where(x => x.IdPerfil == idperfil &&
                                         x.IdPermissao == idpermissao)
                             .FirstOrDefault();
        }

        public List<PerfilPermissao> Consultar(string perfil, int idpermissao)
        {
            return datacontext.PerfilPermissao
                              .Where(x => x.Perfil.Descricao == perfil &&
                                          x.Permissao.IdPai == idpermissao)
                              .ToList();
        }
       
    }
}
