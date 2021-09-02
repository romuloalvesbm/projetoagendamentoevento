using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto.CrossCutting.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Areas.AreaRestrita.Models
{
    public class AgendamentoTurmaEdicaoModel
    {
        public int IdAgendamentoTurma { get; set; }

        public string NomeEvento { get; set; }   
        
        public string DataEvento { get; set; }
       
        public string HoraInicial { get; set; }
       
        public string HoraTermino { get; set; }

        public string DescricaoEvento { get; set; }
       
        public string DataLimiteInscricao { get; set; }        

        public string NomeArea { get; set; }
       
        public string Turma { get; set; }
               
        public string NomeSala { get; set; }

        [Required(ErrorMessage = "Por favor, informe a quantidade de participantes")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, a quantidade deverá se maior {1} ")]
        public int? Parcipantes { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o status.")]
        public string Status { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
    }
}
