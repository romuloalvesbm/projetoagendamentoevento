using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Colaborador
    {
        public int IdColaborador { get; set; }
        public int? IdPerfil { get; set; }
        public string Chapa { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Ramal { get; set; }
        public string Senha { get; set; }

        //Associacoes
        public Perfil Perfil { get; set; }
        public List<AgendamentoColaborador> AgendamentosColaboradores { get; set; }
        
    }
}
