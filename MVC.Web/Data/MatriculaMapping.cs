using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.Web.Models.Entities;

namespace MVC.Web.Data
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matricula");

            builder.HasKey(x => new { x.IdCurso, x.IdAluno});

            builder.HasOne(x => x.Curso)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(x => x.IdCurso);

            builder.HasOne(x => x.Aluno)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(x => x.IdAluno);

            builder.Property(x => x.Id)
                .UseIdentityColumn();
        }
    }
}

