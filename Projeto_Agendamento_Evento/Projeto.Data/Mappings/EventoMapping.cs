using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class EventoMapping : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> buider)
        {
            buider.ToTable("Evento");

            buider.HasKey(e => e.IdEvento)
                  .HasName("PK_EVENTO");

            buider.HasIndex(x => x.Nome)
                 .HasName("UQ_EVENTO_NOME")
                 .IsUnique();
           
            buider.Property(e => e.IdEvento)
              .HasColumnName("IdEvento")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(e => e.Nome)
             .HasColumnName("Nome")
             .HasMaxLength(50)
             .IsRequired();

            buider.Property(e => e.Descricao)
             .HasColumnName("Descricao")
             .HasColumnType("varchar(max)");

            buider.Property(e => e.Desativar)
             .HasColumnName("Desativar")
             .HasMaxLength(1);
        }
    }
}
