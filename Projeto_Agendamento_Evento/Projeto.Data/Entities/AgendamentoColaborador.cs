using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class AgendamentoColaborador
    {
        public int IdAgeCol { get; set; }
        public int IdAgeTurma { get; set; }
        public int IdColaborador { get; set; }

        //Associacoes
        public AgendamentoTurma AgendamentoTurma { get; set; }
        public Colaborador Colaborador { get; set; }
    }
}
