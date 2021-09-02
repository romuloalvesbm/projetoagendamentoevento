using Microsoft.EntityFrameworkCore;
using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Projeto.CrossCutting.Expressions.Contracts;
using Projeto.Data.Dtos;

namespace Projeto.Data.Repositories
{
    public class AgendamentoTurmaRepository : BaseRepository<AgendamentoTurma>, IAgendamentoTurmaRepository
    {
        private readonly DataContext datacontext;

        private readonly ILambdaExpression lambdaexpression;

        public AgendamentoTurmaRepository(DataContext datacontext, ILambdaExpression lambdaexpression) : base(datacontext)
        {
            this.datacontext = datacontext;
            this.lambdaexpression = lambdaexpression;
        }

        public bool ConfirmarEvento(int idsala, DateTime data, TimeSpan hora_inicio, TimeSpan hora_final)
        {
            return datacontext.AgendamentoTurma
                             .Count(x => x.IdSala == idsala &&
                                         x.Data == data &&
                                         x.Status == "Aberto" &&
                                         (hora_inicio <= x.Hora_Fim && hora_final >= x.Hora_Inicio)) > 0;
        }

        public Tuple<List<AgendamentoTurmaDTO>, int, int> ConsultarTodos(AgendamentoTurmaDTO filter, int skip, int take, string sortColumn, string sortDirection)
        {
            var query = datacontext.AgendamentoTurma
                                   .Include(x => x.Sala)
                                   .Include(x => x.Evento)
                                   .Include(x => x.AgendamentosColaboradores)
                                   .Where(x => (filter.Evento == string.Empty || x.Evento.Nome.Contains(filter.Evento)) &&
                                               (filter.Sala == string.Empty || x.Sala.Nome.Contains(filter.Sala)) &&
                                               (filter.Data == null || x.Data == filter.Data) &&
                                               (filter.TotalColaborador == null || x.AgendamentosColaboradores.Count == filter.TotalColaborador) &&
                                               (filter.Status == string.Empty || x.Status.StartsWith(filter.Status)));

            return new Tuple<List<AgendamentoTurmaDTO>, int, int>(
                            lambdaexpression.GetSortedList(query, sortColumn, sortDirection)
                            .Skip(skip).Take(take)
                            .Select(x => new AgendamentoTurmaDTO
                            {
                                IdAgeTurma = x.IdAgeTurma,
                                Evento = x.Evento.Nome,
                                Sala = x.Sala.Nome,
                                Data = x.Data,
                                HoraInicio = x.Hora_Inicio,
                                HoraTermino = x.Hora_Fim,
                                Status = x.Status,
                                TotalColaborador = x.AgendamentosColaboradores.Count()
                            })
                            .ToList(),
                            query.Count(),
                            datacontext.AgendamentoTurma.Count());
        }

        public AgendamentoTurma ObterPor_Id(int id)
        {
            return datacontext.AgendamentoTurma
                             .Include(x => x.Evento)
                             .Include(x => x.AgendamentosColaboradores)
                             .Include(x => x.Area)
                             .Include(x => x.Sala.Localidade)
                             .FirstOrDefault(x => x.IdAgeTurma == id);
        }

    }
}
