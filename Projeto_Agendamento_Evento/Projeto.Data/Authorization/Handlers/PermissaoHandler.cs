using Microsoft.AspNetCore.Authorization;
using Projeto.Data.Authorization.Requirements;
using Projeto.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Data.Authorization.Handlers
{
    public class PermissaoHandler : AuthorizationHandler<PermissaoRequirement>
    {
        private readonly DataContext datacontext;

        public PermissaoHandler(DataContext datacontext)
        {
            this.datacontext = datacontext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissaoRequirement requirement)
        {
            bool autorizacao = false;

            if (context.User.Identity.IsAuthenticated)
            {
                autorizacao = datacontext.PerfilPermissao.Count(x => x.Perfil.Descricao.ToUpper() == context.User.FindFirst(ClaimTypes.Role).Value.ToUpper()
                                                    && x.IdPermissao == requirement.IdPermissao) > 0;
            }

            if (autorizacao) 
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
