using MVC.Web.Models.Entitidades;

namespace MVC.Web.Repositories
{
    public interface ICursoRepository : IBaseRepository<Curso>
    {
        Task<Curso> SelecionarComMatriculasAsync(int id);
    }
}
