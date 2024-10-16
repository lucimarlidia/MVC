using MVC.Web.Models.Entities;
using MVC.Web.Models.ViewModels;

namespace MVC.Web.Services
{
    public interface IAlunoService
    {
        Task<IEnumerable<Aluno>> SelecionarTudo();
        Task<AlunoViewModel> Selecionar(int id);
        Task<int> Salvar(AlunoViewModel alunoViewModel);
        Task<AlunoViewModel> Atualizar(AlunoViewModel alunoViewModel);
        Task<bool> Excluir(int id);
    }
}
