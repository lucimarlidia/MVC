using MVC.Web.Models.Entitidades;
using MVC.Web.Models.ViewModels;
using MVC.Web.Repositories;

namespace MVC.Web.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessorService(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<int> Salvar(ProfessorViewModel professorViewModel)
        {
            if (professorViewModel == null)
            {
                throw new ArgumentNullException(nameof(professorViewModel));
            }

            var professor = professorViewModel.Id != null
                ? await _professorRepository.SelecionarAsync(professorViewModel.Id.Value)
                : null;

            if (professor == null)
            {
                return await InserirProfessorAsync(professorViewModel);
            }

            return await AtualizarProfessorAsync(professor, professorViewModel);
        }


        private async Task<int> InserirProfessorAsync(ProfessorViewModel professorViewModel)
        {
            var professor = new Professor
            {
                Nome = professorViewModel.Nome,
                Email = professorViewModel.Email
            };

            await _professorRepository.IncluirAsync(professor);

            // Retornar o Id do professor recém-inserido
            return professor.Id;
        }

        private async Task<int> AtualizarProfessorAsync(Professor professor, ProfessorViewModel professorViewModel)
        {
            professor.Nome = professorViewModel.Nome;
            professor.Email = professorViewModel.Email;

            await _professorRepository.AlterarAsync(professor);

            // Retornar o Id do professor atualizado
            return professor.Id;
        }
    }
}
