using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class ColaboradorMapping : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> buider)
        {
            buider.ToTable("Colaborador");

            buider.HasKey(x => x.IdColaborador)
                  .HasName("PK_COLABORADOR");

            buider.HasIndex(x => x.Chapa)
                 .HasName("UQ_COLABORADOR_CHAPA")
                 .IsUnique();           

            buider.HasIndex(x => x.Usuario)
                  .HasName("UQ_COLABORADOR_USUARIO")
                  .IsUnique();

            buider.Property(x => x.IdColaborador)
              .HasColumnName("IdColaborador")
              .ValueGeneratedOnAdd()
              .IsRequired();

            buider.Property(x => x.Chapa)
              .HasColumnName("Chapa")
              .HasMaxLength(7)
              .IsRequired();

            buider.Property(x => x.Nome)
              .HasColumnName("Nome")
              .HasMaxLength(100)
              .IsRequired();

            buider.Property(x => x.Usuario)
              .HasColumnName("Usuario")
              .HasMaxLength(30);

            buider.Property(x => x.Email)
              .HasColumnName("Email")
              .HasMaxLength(50)
              .IsRequired();

            buider.Property(x => x.Ramal)
              .HasColumnName("Ramal")
              .HasMaxLength(20);

            buider.Property(x => x.Senha)
             .HasColumnName("Senha")
             .HasMaxLength(50);

            buider.HasOne(x => x.Perfil)
              .WithMany(x => x.Colaboradores)
              .HasForeignKey(x => x.IdPerfil)
              .HasConstraintName("FK_COLABORADOR_PERFIL");
        }
    }
}
