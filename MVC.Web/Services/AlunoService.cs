using MVC.Web.Models.Entitidades;
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

            // Retornar o Id do aluno recém-inserido
            return aluno.Id;
        }

        private async Task<int> AtualizarAlunoAsync(Aluno aluno, AlunoViewModel alunoViewModel)
        {
            aluno.Nome = alunoViewModel.Nome;
            aluno.Endereco = alunoViewModel.Endereco;
            aluno.Email = alunoViewModel.Email;
            aluno.Ativo = alunoViewModel.Ativo;

            await _alunoRepository.AlterarAsync(aluno);

            // Retornar o Id do aluno atualizado
            return aluno.Id;
        }
    }
}
