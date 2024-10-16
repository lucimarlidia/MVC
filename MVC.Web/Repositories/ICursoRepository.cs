using MVC.Web.Models.Entities;

namespace MVC.Web.Repositories
{
    public interface ICursoRepository : IBaseRepository<Curso>
    {
        Task<Curso> SelecionarComMatriculasAsync(int id);
    }
}
