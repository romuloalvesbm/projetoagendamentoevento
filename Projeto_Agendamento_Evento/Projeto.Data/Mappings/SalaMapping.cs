using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Projeto.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projeto.Data.Mappings
{
    public class SalaMapping : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> buider)
        {
            buider.ToTable("Sala");

            buider.HasKey(s => s.IdSala)
                  .HasName("PK_SALA");

            buider.HasIndex(x => new { x.IdLocalidade, x.Nome })
                .HasName("UQ_SALA_NOME")
                .IsUnique();          

            buider.Property(s => s.IdSala)
              .HasColumnName("IdSala")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(s => s.IdLocalidade)
              .HasColumnName("IdLocalidade")
              .IsRequired();

            buider.Property(s => s.Nome)
              .HasColumnName("Nome")
              .HasMaxLength(30)
              .IsRequired();

            buider.Property(s => s.Desativar)
              .HasColumnName("Desativar")
              .HasMaxLength(1);

            buider.HasOne(l => l.Localidade)
                .WithMany(s => s.Salas)
                .HasForeignKey(m => m.IdLocalidade)
                .HasConstraintName("FK_SALA_LOCAL");
        }
    }
}
