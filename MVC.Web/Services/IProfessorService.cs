using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IProfessorService
    {
        Task<int> Salvar(ProfessorViewModel professorViewModel);
        Task<ProfessorViewModel> Selecionar(int id);
        Task<IEnumerable<ProfessorViewModel>> SelecionarTudo();
        Task<ProfessorViewModel> Atualizar(ProfessorViewModel professorViewModel);
        Task<bool> Excluir(int id);
    }
}
