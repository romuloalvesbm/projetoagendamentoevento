using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Area
    {
        public int IdArea { get; set; }
        public string Nome { get; set; }
        public string Desativar { get; set; }

        //Associacoes
        public List<AgendamentoTurma> AgendamentosTurmas { get; set; }
    }
}
