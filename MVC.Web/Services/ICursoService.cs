using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface ICursoService
    {
        Task<int> Salvar(CursoViewModel cursoViewModel);
    }
}
