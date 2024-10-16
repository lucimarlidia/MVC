using MVC.Web.Models.Entities;
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

            return professor.Id;
        }

        private async Task<int> AtualizarProfessorAsync(Professor professor, ProfessorViewModel professorViewModel)
        {
            professor.Nome = professorViewModel.Nome;
            professor.Email = professorViewModel.Email;

            await _professorRepository.AlterarAsync(professor);

            return professor.Id;
        }

        public async Task<ProfessorViewModel> Selecionar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var professor = await _professorRepository.SelecionarAsync(id);
            if (professor == null)
            {
                return null;
            }

            return new ProfessorViewModel
            {
                Id = professor.Id,
                Nome = professor.Nome,
                Email = professor.Email
            };
        }

        public async Task<IEnumerable<ProfessorViewModel>> SelecionarTudo()
        {
            var professores = await _professorRepository.SelecionarTudoAsync();
            return professores.Select(professor => new ProfessorViewModel
            {
                Id = professor.Id,
                Nome = professor.Nome,
                Email = professor.Email
            });
        }

        public async Task<ProfessorViewModel> Atualizar(ProfessorViewModel professorViewModel)
        {
            var professor = await _professorRepository.SelecionarAsync(professorViewModel.Id.Value);
            if (professor == null) return null;

            professor.Nome = professorViewModel.Nome;
            professor.Email = professorViewModel.Email;

            await _professorRepository.AlterarAsync(professor);
            return professorViewModel;
        }

        public async Task<bool> Excluir(int id)
        {
            var professor = await _professorRepository.SelecionarAsync(id);
            if (professor == null) return false;

            await _professorRepository.ExcluirAsync(id);
            return true;
        }
    }
}
