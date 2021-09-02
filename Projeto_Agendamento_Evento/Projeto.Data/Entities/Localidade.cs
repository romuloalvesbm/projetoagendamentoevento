using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Localidade
    {
        public int IdLocalidade { get; set; }
        public string Nome { get; set; }
        public string Desativar { get; set; }

        //Associacoes
        public List<Sala> Salas { get; set; }
    }
}
