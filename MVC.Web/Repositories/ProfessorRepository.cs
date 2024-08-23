using MVC.Web.Data;
using MVC.Web.Models.Entitidades;

namespace MVC.Web.Repositories
{
    public class ProfessorRepository : BaseRepository<Professor>, IProfessorRepository    // BaseRepository implementa o Generics
    {
        public ProfessorRepository(Contexto contexto) : base(contexto)
        {
        }
    }
}