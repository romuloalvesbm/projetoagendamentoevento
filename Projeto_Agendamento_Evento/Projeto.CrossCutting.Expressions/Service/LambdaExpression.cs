using Projeto.CrossCutting.Expressions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projeto.CrossCutting.Expressions.Service
{
    public class LambdaExpression : ILambdaExpression
    {
        public IQueryable<T> GetSortedList<T>(IQueryable<T> source, string sortColumn, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var parts = sortColumn.Split('.');

            Expression parent = param;

            foreach (var part in parts)
            {
                parent = Expression.PropertyOrField(parent, part);
            }

            var expressionLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(parent, typeof(object)), param);

            string methodName = sortDirection == "asc" ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new Type[] { source.ElementType, expressionLambda.Body.Type},
                                  source.Expression, Expression.Quote(expressionLambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
