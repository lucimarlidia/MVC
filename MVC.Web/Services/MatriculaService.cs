using MVC.Web.Models.Entitidades;
using MVC.Web.Models.ViewModels;
using MVC.Web.Repositories;

namespace MVC.Web.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICursoRepository _cursoRepository;

        public MatriculaService(IMatriculaRepository matriculaRepository, IAlunoRepository alunoRepository, ICursoRepository cursoRepository)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<int> Salvar(MatriculaViewModel matriculaViewModel)
        {
            if (matriculaViewModel == null)
            {
                throw new ArgumentNullException(nameof(matriculaViewModel));
            }

            var alunoExiste = await _alunoRepository.SelecionarAsync(matriculaViewModel.IdAluno);
            if (alunoExiste == null)
            {
                throw new ArgumentException("O aluno informado não existe.");
            }
            if (!alunoExiste.Ativo)
            {
                throw new ArgumentException("O aluno informado não está ativo.");
            }

            var cursoExiste = await _cursoRepository.SelecionarComMatriculasAsync(matriculaViewModel.IdCurso);
            if (cursoExiste == null)
            {
                throw new ArgumentException("O curso informado não existe.");
            }
            if (!cursoExiste.Ativo)
            {
                throw new ArgumentException("O curso informado não está ativo.");
            }
            if (cursoExiste.DataInicio.Date <= DateTime.UtcNow.Date)
            {
                throw new ArgumentException("O curso informado já foi iniciado. Matrícula não permitida.");
            }

            if (cursoExiste.Matriculas is not null)
            {
                if (cursoExiste.Matriculas.Count(m => m.StatusMatricula == StatusMatricula.Ativa) >= 30)
                {
                    throw new InvalidOperationException("O curso informado já atingiu o limite máximo de 30 matrículas ativas.");
                }

                if (cursoExiste.Matriculas.Exists(m => m.IdAluno == matriculaViewModel.IdAluno && m.StatusMatricula == StatusMatricula.Ativa))
                {
                    throw new InvalidOperationException("O aluno já está matriculado no curso com uma matrícula ativa.");
                }
            }

            var matricula = matriculaViewModel.Id != null
                ? await _matriculaRepository.SelecionarAsync(matriculaViewModel.Id.Value)
                : null;

            if (matricula == null)
            {
                return await InserirMatriculaAsync(matriculaViewModel);
            }

            return await AtualizarMatriculaAsync(matricula, matriculaViewModel);
        }

        private async Task<int> InserirMatriculaAsync(MatriculaViewModel matriculaViewModel)
        {
            var matricula = new Matricula
            {
                IdAluno = matriculaViewModel.IdAluno,
                IdCurso = matriculaViewModel.IdCurso,
                StatusMatricula = StatusMatricula.Ativa,
            };

            await _matriculaRepository.IncluirAsync(matricula);
            return matricula.Id;
        }

        private async Task<int> AtualizarMatriculaAsync(Matricula matricula, MatriculaViewModel matriculaViewModel)
        {
            matricula.IdAluno = matriculaViewModel.IdAluno;
            matricula.IdCurso = matriculaViewModel.IdCurso;
            matricula.StatusMatricula = matriculaViewModel.StatusMatricula;

            await _matriculaRepository.AlterarAsync(matricula);
            return matricula.Id;
        }

        public async Task<MatriculaViewModel> Selecionar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido.", nameof(id));
            }

            var matricula = await _matriculaRepository.SelecionarAsync(id);
            if (matricula == null)
            {
                return null;
            }

            return new MatriculaViewModel
            {
                Id = matricula.Id,
                IdAluno = matricula.IdAluno,
                IdCurso = matricula.IdCurso,
                StatusMatricula = matricula.StatusMatricula
            };
        }

        public async Task<IEnumerable<MatriculaViewModel>> SelecionarTudo()
        {
            var matriculas = await _matriculaRepository.SelecionarTudoAsync();
            return matriculas.Select(matricula => new MatriculaViewModel
            {
                Id = matricula.Id,
                IdAluno = matricula.IdAluno,
                IdCurso = matricula.IdCurso,
                StatusMatricula = matricula.StatusMatricula
            });
        }

        public async Task<MatriculaViewModel> Atualizar(MatriculaViewModel matriculaViewModel)
        {
            var matricula = await _matriculaRepository.SelecionarAsync(matriculaViewModel.Id.Value);
            if (matricula == null) return null;

            matricula.IdAluno = matriculaViewModel.IdAluno;
            matricula.IdCurso = matriculaViewModel.IdCurso;
            matricula.StatusMatricula = matriculaViewModel.StatusMatricula;

            await _matriculaRepository.AlterarAsync(matricula);
            return matriculaViewModel;
        }

        public async Task<bool> Excluir(int id)
        {
            var matricula = await _matriculaRepository.SelecionarAsync(id);
            if (matricula == null)
            {
                return false;
            }

            await _matriculaRepository.ExcluirAsync(id);
            return true;
        }
    }
}
