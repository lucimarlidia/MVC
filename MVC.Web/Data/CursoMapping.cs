using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.Web.Models.Entities;

namespace MVC.Web.Data
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Curso");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.Descricao)
               .HasColumnType("varchar(500)");

            builder.Property(x => x.Ativo)
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(x => x.DataInicio)
               .HasColumnType("datetime")
               .IsRequired();

            builder.HasOne(t => t.Professor)
                .WithMany(tp => tp.Cursos)
                .HasForeignKey(i => i.IdProfessor);
        }
    }
}
