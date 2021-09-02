using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IEventoRepository : IBaseRepository<Evento>
    {        
        int? ObterIdPorNome(string nome);
        List<Evento> ConsultarAtivoPorNome(string nome);
        bool EventoAtivo(int idevento);
    }
}
