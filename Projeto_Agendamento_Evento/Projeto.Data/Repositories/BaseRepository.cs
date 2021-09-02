using Microsoft.EntityFrameworkCore;
using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        private readonly DataContext dataContext;

        public BaseRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Inserir(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Added; //inserção
            dataContext.SaveChanges();
        }

        public void Alterar(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Modified; //edição
            dataContext.SaveChanges();
        }

        public void Excluir(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Deleted; //exclusão
            dataContext.SaveChanges();
        }

        public List<T> Consultar()
        {
            return dataContext.Set<T>().ToList();
        }

        public T ObterPorId(int id)
        {
            return dataContext.Set<T>()
                              .Find(id); //buscar pelo id..
        }
    }
}
