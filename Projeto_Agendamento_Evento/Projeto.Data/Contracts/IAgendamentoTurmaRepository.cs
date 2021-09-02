using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IAgendamentoTurmaRepository : IBaseRepository<AgendamentoTurma>
    {
        bool ConfirmarEvento(int idsala, DateTime data, TimeSpan hora_inicio, TimeSpan hora_final);
        Tuple<List<AgendamentoTurmaDTO>, int, int> ConsultarTodos(AgendamentoTurmaDTO filter,  
                                                                  int skip, int take, string sortColumn, string sortDirection);
        AgendamentoTurma ObterPor_Id(int id);
    }
}
