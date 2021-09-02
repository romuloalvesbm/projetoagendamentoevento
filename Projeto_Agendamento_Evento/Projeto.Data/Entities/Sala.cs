using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Sala
    {
        public int IdSala { get; set; }
        public int IdLocalidade { get; set; }
        public string Nome { get; set; }
        public string Desativar { get; set; }

        //Associacoes
        public Localidade Localidade { get; set; }
        public List<AgendamentoTurma> AgendamentosTurmas { get; set; }
    }
}
