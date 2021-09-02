using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Projeto.CrossCutting.Validations;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class AgendamentoTurmaCadastroModel
    {
        [Required(ErrorMessage = "Por favor, informe o nome do evento.")]
        public int? IdEvento { get; set; }

        public string NomeEvento { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data do evento.")]
        public DateTime? DataEvento { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora início do evento.")]
        [Range(typeof(TimeSpan), "00:01", "23:59", ErrorMessage = "Intervalo válido" +
                                         " de 00:01 e 23:59.")]
        public TimeSpan? HoraInicial { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora término do evento.")]
        [Range(typeof(TimeSpan), "00:01", "23:59", ErrorMessage = "Intervalo válido" +
                                         " de 00:01 e 23:59.")]
        [TimeGreaterThan("HoraInicial")]
        public TimeSpan? HoraTermino { get; set; }

        public string DescricaoEvento { get; set; }

        [Required(ErrorMessage = "Por favor, informe o limite de inscrição.")]
        [DateLessThan("DataEvento")]
        public DateTime? DataLimiteInscrição { get; set; }        
        [Required(ErrorMessage = "Por favor, informe o nome da area.")]
        public int? IdArea { get; set; }

        public string NomeArea { get; set; }

        [MaxLength(30, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a descrição da tumra.")]
        public string Turma { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da sala.")]
        public int? IdSala { get; set; }

        public string NomeSala { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe a quantidade de participantes")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, a quantidade deverá se maior {1} ")]
        public int? Parcipantes { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o status.")]
        public string Status { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
    }
}
