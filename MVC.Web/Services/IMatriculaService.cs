using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IMatriculaService
    {
        Task<int> Salvar(MatriculaViewModel matriculaViewModel);
    }
}
