using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
        public string Bloqueado { get; set; }
        public string Desativar { get; set; }
        public List<PerfilPermissao> Perfis_Permissoes { get; set; }
        public List<Colaborador> Colaboradores { get; set; }
    }
}
