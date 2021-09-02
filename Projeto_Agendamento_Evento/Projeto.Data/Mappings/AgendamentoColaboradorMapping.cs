using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class AgendamentoColaboradorMapping : IEntityTypeConfiguration<AgendamentoColaborador>
    {
        public void Configure(EntityTypeBuilder<AgendamentoColaborador> buider) 
        {
            buider.ToTable("Agendamento_Colaborador");

            buider.HasKey(x => x.IdAgeCol)
                  .HasName("PK_AGECOLAB");

            buider.HasAlternateKey(x => new { x.IdAgeTurma, x.IdColaborador })
                  .HasName("UQ_AGECOLAB_TURMA_COLAB");

            buider.Property(x => x.IdAgeCol)
               .HasColumnName("IdAgeCol")
               .ValueGeneratedOnAdd()
               .IsRequired();

            buider.Property(x => x.IdAgeTurma)
                .HasColumnName("IdAgeTurma")
                .IsRequired();

            buider.Property(x => x.IdColaborador)
                .HasColumnName("IdColaborador")
                .IsRequired();

            //mapeamento do relacionamento.. 
            buider.HasOne(x => x.AgendamentoTurma)
                .WithMany(x => x.AgendamentosColaboradores)
                .HasForeignKey(x => x.IdAgeTurma)
                .HasConstraintName("FK_AGECOL_COLABORADOR");

            buider.HasOne(x => x.Colaborador)
                .WithMany(x => x.AgendamentosColaboradores)
                .HasForeignKey(x => x.IdColaborador)
                .HasConstraintName("FK_AGECOL_AGETURMA");
        }
    }
}
