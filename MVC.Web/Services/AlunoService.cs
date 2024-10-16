using MVC.Web.Models.Entities;
using MVC.Web.Models.ViewModels;
using MVC.Web.Repositories;

namespace MVC.Web.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<int> Salvar(AlunoViewModel alunoViewModel)
        {
            if (alunoViewModel == null)
            {
                throw new ArgumentNullException(nameof(alunoViewModel));
            }

            var aluno = alunoViewModel.Id != null
                ? await _alunoRepository.SelecionarAsync(alunoViewModel.Id.Value)
                : null;

            if (aluno == null)
            {
                return await InserirAlunoAsync(alunoViewModel);
            }

            return await AtualizarAlunoAsync(aluno, alunoViewModel);
        }

        private async Task<int> InserirAlunoAsync(AlunoViewModel alunoViewModel)
        {
            var aluno = new Aluno
            {
                Nome = alunoViewModel.Nome,
                Endereco = alunoViewModel.Endereco,
                Email = alunoViewModel.Email,
                Ativo = true
            };

            await _alunoRepository.IncluirAsync(aluno);

            return aluno.Id;
        }

        private async Task<int> AtualizarAlunoAsync(Aluno aluno, AlunoViewModel alunoViewModel)
        {
            aluno.Nome = alunoViewModel.Nome;
            aluno.Endereco = alunoViewModel.Endereco;
            aluno.Email = alunoViewModel.Email;
            aluno.Ativo = alunoViewModel.Ativo;

            await _alunoRepository.AlterarAsync(aluno);

            return aluno.Id;
        }

        public async Task<AlunoViewModel> Selecionar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var aluno = await _alunoRepository.SelecionarAsync(id);

            if (aluno == null)
            {
                return null;
            }

            var alunoViewModel = new AlunoViewModel
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Endereco = aluno.Endereco,
                Email = aluno.Email,
                Ativo = aluno.Ativo
            };

            return alunoViewModel;
        }

        public async Task<IEnumerable<Aluno>> SelecionarTudo()
        {
            return await _alunoRepository.SelecionarTudoAsync();
        }

        public async Task<AlunoViewModel> Atualizar(AlunoViewModel alunoViewModel)
        {
            var aluno = await _alunoRepository.SelecionarAsync(alunoViewModel.Id.Value);
            if (aluno == null) return null;

            aluno.Nome = alunoViewModel.Nome;
            aluno.Endereco = alunoViewModel.Endereco;
            aluno.Email = alunoViewModel.Email;
            aluno.Ativo = alunoViewModel.Ativo;

            await _alunoRepository.AlterarAsync(aluno);
            return alunoViewModel;
        }

        public async Task<bool> Excluir(int id)
        {
            var aluno = await _alunoRepository.SelecionarAsync(id);
            if (aluno == null) return false;

            await _alunoRepository.ExcluirAsync(id);
            return true;
        }
    }
}
