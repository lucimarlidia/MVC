using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IProfessorService
    {
        Task<int> Salvar(ProfessorViewModel professorViewModel);
    }
}
