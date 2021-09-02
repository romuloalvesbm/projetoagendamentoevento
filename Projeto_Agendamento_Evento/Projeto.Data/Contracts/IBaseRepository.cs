using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IBaseRepository<T>
        where T : class
    {
        void Inserir(T entity);
        void Alterar(T entity);
        void Excluir(T entity);
        List<T> Consultar();
        T ObterPorId(int id);
    }
}
