using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IColaboradorRepository : IBaseRepository<Colaborador>
    {
        bool PasswordExiste(string usuario);
        bool PerfilExiste(string usuario);
        Colaborador Consultar(string usuario, string password);
    }
}
