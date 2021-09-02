using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IAreaRepository : IBaseRepository<Area>
    {       
        int? ObterIdPorNome(string nome);
        List<Area> ConsultarAtivoPorNome(string nome);
        bool AreaAtiva(int idarea);
    }
}
