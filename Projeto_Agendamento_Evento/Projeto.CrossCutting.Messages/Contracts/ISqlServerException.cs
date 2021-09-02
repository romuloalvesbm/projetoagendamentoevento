using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.CrossCutting.Messages.Contracts
{
    public interface ISqlServerException
    {
        string AlterarDescricao(int code);
    }
}
