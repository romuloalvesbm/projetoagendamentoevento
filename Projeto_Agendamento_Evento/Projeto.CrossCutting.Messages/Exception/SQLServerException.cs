using Projeto.CrossCutting.Messages.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.CrossCutting.Messages.Exception
{
    public class SQLServerException : ISqlServerException
    {
        public string AlterarDescricao(int code)
        {
            string message = string.Empty;

            switch (code)
            {
                case 547:
                    message = "Existe(m) informações relacionada(s) a este registro que impede sua exclusão.";
                    break;
            }

            return message;
        }
    }
}
