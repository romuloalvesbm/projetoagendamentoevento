using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Entities
{
    public class Permissao
    {
        public int IdPermissao { get; set; }
        public string Descricao { get; set; }
        public string Bloqueado { get; set; }
        public int? IdPai { get; set; }
        public string Categoria { get; set; }

        public Permissao PermissaoPai { get; set; }
        public List<Permissao> Permissoes { get; set; }
        public virtual List<PerfilPermissao> Perfis_Permissoes { get; set; }
    }
}
