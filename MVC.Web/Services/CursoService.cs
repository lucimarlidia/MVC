using MVC.Web.Models.Entitidades;
using MVC.Web.Models.ViewModels;
using MVC.Web.Repositories;

namespace MVC.Web.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IProfessorRepository _professorRepository;

        public CursoService(ICursoRepository cursoRepository, IProfessorRepository professorRepository)
        {
            _cursoRepository = cursoRepository;
            _professorRepository = professorRepository;
        }

        public async Task<int> Salvar(CursoViewModel cursoViewModel)
        {
            if (cursoViewModel == null)
            {
                throw new ArgumentNullException(nameof(cursoViewModel));
            }

            // Validação do IdProfessor
            var professorExiste = await _professorRepository.SelecionarAsync(cursoViewModel.IdProfessor);
            if (professorExiste == null)
            {
                throw new ArgumentException("O professor informado não existe.");
            }

            var curso = cursoViewModel.Id != null
                ? await _cursoRepository.SelecionarAsync(cursoViewModel.Id.Value)
                : null;

            if (curso == null)
            {
                return await InserirCursoAsync(cursoViewModel);
            }

            return await AtualizarCursoAsync(curso, cursoViewModel);
        }


        private async Task<int> InserirCursoAsync(CursoViewModel cursoViewModel)
        {
            var curso = new Curso
            {
                Titulo = cursoViewModel.Titulo,
                Descricao = cursoViewModel.Descricao,
                Ativo = true,
                DataInicio = cursoViewModel.DataInicio,
                IdProfessor = cursoViewModel.IdProfessor
            };

            await _cursoRepository.IncluirAsync(curso);

            // Retornar o Id do curso recém-inserido
            return curso.Id;
        }

        private async Task<int> AtualizarCursoAsync(Curso curso, CursoViewModel cursoViewModel)
        {
            curso.Titulo = cursoViewModel.Titulo;
            curso.Descricao = cursoViewModel.Descricao;
            curso.Ativo = cursoViewModel.Ativo;
            curso.DataInicio = cursoViewModel.DataInicio;
            curso.IdProfessor = cursoViewModel.IdProfessor;

            await _cursoRepository.AlterarAsync(curso);

            // Retornar o Id do curso atualizado
            return curso.Id;
        }
    }
}
