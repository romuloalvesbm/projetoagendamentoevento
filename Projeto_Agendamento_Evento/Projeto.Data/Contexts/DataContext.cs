using Microsoft.EntityFrameworkCore;
using Projeto.Data.Entities;
using Projeto.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurar o campos string em varchar
            //foreach (var property in modelBuilder.Model.GetEntityTypes()
            //                                     .SelectMany(t => t.GetProperties())
            //                                     .Where(p => p.ClrType == typeof(string)))
            //{
            //    property.Relational().ColumnType = "nvarchar";
            //}

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                                                     .Where(e => !e.IsOwned())
                                                     .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new AgendamentoColaboradorMapping());
            modelBuilder.ApplyConfiguration(new AgendamentoTurmaMapping());
            modelBuilder.ApplyConfiguration(new AreaMapping());
            modelBuilder.ApplyConfiguration(new ColaboradorMapping());
            modelBuilder.ApplyConfiguration(new EventoMapping());
            modelBuilder.ApplyConfiguration(new LocalidadeMapping());
            modelBuilder.ApplyConfiguration(new PerfilMapping());
            modelBuilder.ApplyConfiguration(new PerfilPermissaoMapping());
            modelBuilder.ApplyConfiguration(new PermissaoMapping());
            modelBuilder.ApplyConfiguration(new SalaMapping());
        }

        public DbSet<AgendamentoColaborador> AgendamentoColaborador { get; set; }
        public DbSet<AgendamentoTurma> AgendamentoTurma { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Colaborador> Colaborador { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Localidade> Localidade { get; set; }
        public DbSet<PerfilPermissao> PerfilPermissao { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Permissao> Permissao { get; set; }
        public DbSet<Sala> Sala { get; set; }
    }
}
