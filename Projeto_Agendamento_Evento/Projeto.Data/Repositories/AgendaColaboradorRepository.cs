using Microsoft.EntityFrameworkCore;
using Projeto.CrossCutting.Expressions.Contracts;
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
    public class AgendaColaboradorRepository : BaseRepository<AgendamentoColaborador>, IAgendamentoColaboradorRepository
    {
        private readonly DataContext datacontext;
        private readonly ILambdaExpression lambdaexpression;

        public AgendaColaboradorRepository(DataContext datacontext, ILambdaExpression lambdaexpression) : base(datacontext)
        {
            this.datacontext = datacontext;
            this.lambdaexpression = lambdaexpression;
        }

        public Tuple<List<AgendamentoColaboradorDto>, int, int> ListarPorTurma(int idageturma, string chapa, string nome, int skip, int take, string sortColumn, string sortDirection) 
        {
            var query = datacontext.AgendamentoColaborador
                                   .Include(x => x.Colaborador)
                                   .Where(x => (chapa == string.Empty || x.Colaborador.Chapa.StartsWith(chapa)) &&
                                               (nome == string.Empty || x.Colaborador.Nome.StartsWith(nome)) &&
                                          x.IdAgeTurma == idageturma);


            return new Tuple<List<AgendamentoColaboradorDto>, int, int>(
                            lambdaexpression.GetSortedList(query, sortColumn, sortDirection)
                            .Skip(skip).Take(take)
                            .Select(x => new AgendamentoColaboradorDto
                            {
                                IdAgeCol = x.IdAgeCol,
                                chapa = x.Colaborador.Chapa,
                                Nome = x.Colaborador.Nome
                            })
                            .ToList(),
                            query.Count(),
                            datacontext.AgendamentoColaborador.Count(x => x.IdAgeTurma == idageturma));
        }


    }
}
