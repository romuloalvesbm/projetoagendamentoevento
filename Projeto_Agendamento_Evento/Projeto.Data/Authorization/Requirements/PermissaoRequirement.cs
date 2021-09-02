using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Authorization.Requirements
{
    public class PermissaoRequirement : IAuthorizationRequirement
    {
        public PermissaoRequirement(int idPermissao)
        {
            IdPermissao = idPermissao;
        }

        public int IdPermissao { get; set; }
    }
}
