using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projeto.CrossCutting.Expressions.Contracts
{
    public interface ILambdaExpression
    {
        IQueryable<T> GetSortedList<T>(IQueryable<T> source, string sortColumn, string sortDirection);
    }
}
