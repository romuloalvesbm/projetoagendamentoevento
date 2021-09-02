using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class AgendamentoTurmaMapping : IEntityTypeConfiguration<AgendamentoTurma>
    {
        public void Configure(EntityTypeBuilder<AgendamentoTurma> buider)
        {
            buider.ToTable("Agendamento_Turma");

            buider.HasKey(x => x.IdAgeTurma)
                  .HasName("PK_AGENTURMA");

            buider.Property(x => x.IdAgeTurma)
                .HasColumnName("IdAgeTurma")
                .ValueGeneratedOnAdd()
                .IsRequired();

            buider.Property(x => x.IdEvento)
               .HasColumnName("IdEvento")
               .IsRequired();

            buider.Property(x => x.IdArea)
              .HasColumnName("IdArea")
              .IsRequired();

            buider.Property(x => x.IdSala)
              .HasColumnName("IdSala")
              .IsRequired();

            buider.Property(x => x.Turma)
               .HasColumnName("Turma")
               .HasMaxLength(30)
               .IsRequired();

            buider.Property(x => x.Data)
               .HasColumnName("Data")
               .IsRequired();

            buider.Property(x => x.Hora_Inicio)
              .HasColumnName("Hora_Inicio")
              .IsRequired();

            buider.Property(x => x.Hora_Fim)
              .HasColumnName("Hora_Fim")
              .IsRequired();

            buider.Property(x => x.Data_Limite)
              .HasColumnName("Data_Limite")
              .IsRequired();

            buider.Property(x => x.Max_Participante)
              .HasColumnName("Max_Participante")
              .IsRequired();

            buider.Property(x => x.Status)
               .HasColumnName("Status")
               .HasMaxLength(30)
               .IsRequired();
           
            buider.HasOne(e => e.Evento)
                  .WithMany(x => x.AgendamentosTurmas)
                  .HasForeignKey(x => x.IdEvento)
                  .HasConstraintName("FK_AGENTURMA_EVENTO");

            buider.HasOne(x => x.Area)
               .WithMany(x => x.AgendamentosTurmas)
               .HasForeignKey(x => x.IdArea)
               .HasConstraintName("FK_AGENTURMA_AREA");

            buider.HasOne(s => s.Sala)
               .WithMany(x => x.AgendamentosTurmas)
               .HasForeignKey(x => x.IdSala)
               .HasConstraintName("FK_AGENTURMA_SALA");
        }
    }
}
