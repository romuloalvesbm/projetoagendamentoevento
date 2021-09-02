using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projeto.CrossCutting.Expressions.Contracts;
using Projeto.CrossCutting.Expressions.Service;
using Projeto.CrossCutting.Messages.Contracts;
using Projeto.CrossCutting.Messages.Exception;
using Projeto.Data.Authorization.Handlers;
using Projeto.Data.Authorization.Requirements;
using Projeto.Data.Contexts;
using Projeto.Data.Contracts;
using Projeto.Data.Repositories;

namespace Projeto.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                                           Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            #region Autorizacao

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissaoArea",
                                  policy => policy.Requirements.Add(new PermissaoRequirement(1)));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissaoEvento",
                                  policy => policy.Requirements.Add(new PermissaoRequirement(5)));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissaoLocalidade",
                                  policy => policy.Requirements.Add(new PermissaoRequirement(9)));
            });
            #endregion

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddDbContext<DataContext>
                    (options => options.UseSqlServer
                    (Configuration.GetConnectionString("Conexao")));

            services.AddTransient<IColaboradorRepository, ColaboradorRepository>();
            services.AddTransient<IPerfilRepository, PerfilRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IAreaRepository, AreaRepository>();
            services.AddTransient<ILocalidadeRepository, LocalidadeRepository>();
            services.AddTransient<ISalaRepository, SalaRepository>();
            services.AddTransient<IAgendamentoColaboradorRepository, AgendaColaboradorRepository>();
            services.AddTransient<IAgendamentoTurmaRepository, AgendamentoTurmaRepository>();
            services.AddTransient<IPermissaoRepository, PermissaoRepository>();
            services.AddTransient<IPerfilPermissaoRepository, PerfilPermissaoRepository>();

            services.AddTransient<IAuthorizationHandler, PermissaoHandler>();
            services.AddTransient<ISqlServerException, SQLServerException>();
            services.AddTransient<ILambdaExpression, LambdaExpression>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(
                routes =>
                {
                    //mapeamento do conteúdo da área restrita do projeto
                    routes.MapRoute(
                        name: "areas", //mapear uma subarea do projeto
                        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    routes.MapRoute(
                        name: "default", //caminho de página padrão do sistema
                        template: "{controller=Account}/{action=Login}/{id?}");
                    //ROTA default: /Account/Login
                });
        }
    }
}
