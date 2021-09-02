using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Projeto.CrossCutting.Validations
{
    public static class Date
    {
        public static DateTime Validaticao(string data)
        {
            string[] formats = { "dd/MM/yyyy" };

            DateTime dataAux;
            if (DateTime.TryParseExact(data, formats, new CultureInfo("pt-BR"),
                                        DateTimeStyles.None, out dataAux))
                return dataAux;
            else
                throw new Exception("Data Inválida");
        }
    }
}
