using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IAgendamentoColaboradorRepository : IBaseRepository<AgendamentoColaborador>
    {
        Tuple<List<AgendamentoColaboradorDto>, int, int> ListarPorTurma(int idageturma, string chapa, string nome, int skip, 
                                                                     int take, string sortColumn, string sortDirection);
    }
}
