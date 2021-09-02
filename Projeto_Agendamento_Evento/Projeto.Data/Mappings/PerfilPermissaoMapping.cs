using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class PerfilPermissaoMapping : IEntityTypeConfiguration<PerfilPermissao>
    {
        public void Configure(EntityTypeBuilder<PerfilPermissao> buider)
        {
            buider.ToTable("Perfil_Permissao");

            buider.HasKey(x => x.Id)
                  .HasName("PK_PERFIL_PERMISSAO");

            buider.HasAlternateKey(x => new { x.IdPerfil, x.IdPermissao })
                 .HasName("UQ_PERFIL_PERMISSAO");

            buider.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            buider.Property(x => x.IdPerfil)
               .HasColumnName("IdPerfil")
               .IsRequired();

            buider.Property(x => x.IdPermissao)
               .HasColumnName("IdPermissao")
               .IsRequired();

            buider.HasOne(x => x.Perfil)
                .WithMany(x => x.Perfis_Permissoes)
                .HasForeignKey(x => x.IdPerfil)
                .OnDelete(DeleteBehavior.Cascade);

            buider.HasOne(x => x.Permissao)
                .WithMany(x => x.Perfis_Permissoes)
                .HasForeignKey(x => x.IdPermissao);
        }
    }
}
