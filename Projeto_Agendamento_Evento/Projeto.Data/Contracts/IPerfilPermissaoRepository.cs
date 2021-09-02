using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IPerfilPermissaoRepository : IBaseRepository<PerfilPermissao>
    {
        bool PermissaoAutorizada(string perfil, int idpermissao);

        PerfilPermissao PermissaoAutorizada(int idperfil, int idpermissao);

        List<PerfilPermissao> Consultar(string perfil, int idpermissao);
    }
}
