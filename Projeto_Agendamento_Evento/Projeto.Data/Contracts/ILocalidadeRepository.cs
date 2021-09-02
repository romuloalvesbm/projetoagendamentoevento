using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface ILocalidadeRepository : IBaseRepository<Localidade>
    {        
        int? ObterIdPorNome(string nome);
        List<Localidade> ConsultarAtivoPorNome(string nome);
        List<Localidade> ConsultarAtivo();
    }
}
