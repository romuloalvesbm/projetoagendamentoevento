using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class LocalidadeMapping : IEntityTypeConfiguration<Localidade>
    {
        public void Configure(EntityTypeBuilder<Localidade> buider) 
        {
            buider.ToTable("Localidade");

            buider.HasKey(x => x.IdLocalidade)
                  .HasName("PK_LOCALIDADE");

            buider.HasIndex(x => x.Nome)
                 .HasName("UQ_LOCALIDADE_NOME")
                 .IsUnique();
           
            buider.Property(x => x.IdLocalidade)
              .HasColumnName("IdLocalidade")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(x => x.Nome)
              .HasColumnName("Nome")
              .HasMaxLength(50)
              .IsRequired();

            buider.Property(x => x.Desativar)
              .HasColumnName("Desativar")
              .HasMaxLength(1);
        }
    }
}
