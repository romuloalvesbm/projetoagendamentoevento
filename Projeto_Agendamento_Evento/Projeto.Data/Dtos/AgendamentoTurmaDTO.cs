using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Dtos
{
    public class AgendamentoTurmaDTO
    {
        public int IdAgeTurma { get; set; }
        public string Evento { get; set; }
        public string Sala { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public string Status { get; set; }
        public int? TotalColaborador { get; set; }       
    }
}
