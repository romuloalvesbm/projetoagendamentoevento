using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface ISalaRepository : IBaseRepository<Sala>
    {              
        int? ObterIdPorCriterio(string nome, int idlocalidade);
        List<Sala> ConsultarAtivoPorNome(string nome);       
        List<Sala> ConsultarTodos();
        bool SalaAtiva(int idsala);
    }
}
