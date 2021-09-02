using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class PermissaoMapping : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> buider)
        {
            buider.ToTable("Permissao");

            buider.HasKey(x => x.IdPermissao)
                  .HasName("PK_PERMISSAO");

            buider.Property(x => x.IdPermissao)
              .HasColumnName("IdPermissao")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(x => x.Descricao)
             .HasColumnName("Descricao")
             .HasMaxLength(50)
             .IsRequired();

            buider.Property(x => x.Bloqueado)
            .HasColumnName("Bloqueado")
            .HasMaxLength(1);

            buider.Property(x => x.IdPai)
            .HasColumnName("IdPai");

            buider.Property(x => x.Categoria)
            .HasColumnName("Categoria")
            .HasMaxLength(100);

            buider.HasOne(x => x.PermissaoPai)
               .WithMany(x => x.Permissoes)
               .HasForeignKey(x => x.IdPai)
               .HasConstraintName("FK_PERMISSAO");
        }
    }
}
