using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class AgendamentoTurma
    {
        public int IdAgeTurma { get; set; }
        public int IdEvento { get; set; }
        public int IdArea { get; set; }
        public int IdSala { get; set; }
        public string Turma { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora_Inicio { get; set; }
        public TimeSpan Hora_Fim { get; set; }
        public DateTime Data_Limite { get; set; }
        public int Max_Participante { get; set; }
        public string Status { get; set; }

        //Associacoes
        public Evento Evento { get; set; }
        public Area Area { get; set; }
        public Sala Sala { get; set; }
        public List<AgendamentoColaborador> AgendamentosColaboradores { get; set; }
    }
}
