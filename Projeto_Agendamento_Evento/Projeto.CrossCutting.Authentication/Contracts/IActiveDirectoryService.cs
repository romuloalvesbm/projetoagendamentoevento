using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.CrossCutting.Authentication.Contracts
{
    public interface IActiveDirectoryService
    {
        bool Autenticacao(string domain, string username, string password);
    }
}
