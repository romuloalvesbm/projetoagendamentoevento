using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class PerfilMapping : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> buider) 
        {
            buider.ToTable("Perfil");

            buider.HasKey(x => x.IdPerfil)
                  .HasName("PK_PERFIL");

            buider.Property(x => x.IdPerfil)
              .HasColumnName("IdPerfil")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(x => x.Descricao)
              .HasColumnName("Descricao")
              .HasMaxLength(50)
              .IsRequired();

            buider.Property(a => a.Desativar)
              .HasColumnName("Desativar")
              .HasMaxLength(1);
        }
    }
}
