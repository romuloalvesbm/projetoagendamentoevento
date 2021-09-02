using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class PerfilPermissao
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public int IdPermissao { get; set; }

        public Perfil Perfil { get; set; }
        public Permissao Permissao { get; set; }
    }
}
