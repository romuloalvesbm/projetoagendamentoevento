using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projeto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Mappings
{
    public class AreaMapping : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> buider)
        {
            buider.ToTable("Area");

            buider.HasKey(x => x.IdArea)
                 .HasName("PK_AREA");

            buider.HasIndex(x => x.Nome)
                  .HasName("UQ_AREA_NOME")
                  .IsUnique();          

            buider.Property(x => x.IdArea)
              .HasColumnName("IdArea")
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
