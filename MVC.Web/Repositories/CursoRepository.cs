using Microsoft.EntityFrameworkCore;
using MVC.Web.Data;
using MVC.Web.Models.Entitidades;

namespace MVC.Web.Repositories
{
    public class CursoRepository : BaseRepository<Curso>, ICursoRepository  // BaseRepository implementa o Generics
    {
        public CursoRepository(Contexto contexto) : base(contexto)
        {
        }

        public async Task<Curso> SelecionarComMatriculasAsync(int id)
        {
            return await _contexto.Set<Curso>()
                .Include(c => c.Matriculas) // Carrega as matrículas associadas
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}