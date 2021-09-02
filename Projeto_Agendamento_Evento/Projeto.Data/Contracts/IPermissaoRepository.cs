using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IPermissaoRepository : IBaseRepository<Permissao>
    {
        List<Permissao> ObterPai();
    }
}
