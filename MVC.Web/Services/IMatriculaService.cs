using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IMatriculaService
    {
        Task<int> Salvar(MatriculaViewModel matriculaViewModel);
        Task<MatriculaViewModel> Selecionar(int id);
        Task<IEnumerable<MatriculaViewModel>> SelecionarTudo();
        Task<MatriculaViewModel> Atualizar(MatriculaViewModel matriculaViewModel);
        Task<bool> Excluir(int id);
    }
}
