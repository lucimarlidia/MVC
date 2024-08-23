using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IAlunoService
    {
        Task<int> Salvar(AlunoViewModel alunoViewModel);
    }
}
