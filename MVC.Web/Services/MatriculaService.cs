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

            await ValidarAlunoAsync(matriculaViewModel.IdAluno);
            await ValidarCursoAsync(matriculaViewModel.IdCurso, matriculaViewModel.IdAluno);

            var matricula = matriculaViewModel.Id != null
                ? await _matriculaRepository.SelecionarAsync(matriculaViewModel.Id.Value)
                : null;

            if (matricula == null)
            {
                return await InserirMatriculaAsync(matriculaViewModel);
            }

            return await AtualizarMatriculaAsync(matricula, matriculaViewModel);
        }

        private async Task ValidarAlunoAsync(int idAluno)
        {
            var alunoExiste = await _alunoRepository.SelecionarAsync(idAluno);
            if (alunoExiste == null)
            {
                throw new ArgumentException("O aluno informado não existe.");
            }
            if (!alunoExiste.Ativo)
            {
                throw new ArgumentException("O aluno informado não está ativo.");
            }
        }

        private async Task ValidarCursoAsync(int idCurso, int idAluno)
        {
            var cursoExiste = await _cursoRepository.SelecionarComMatriculasAsync(idCurso);
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
                throw new ArgumentException("O curso informado já foi iniciado, matrícula não permitida.");
            }

            if (cursoExiste.Matriculas is not null)
            {
                // Validação de limite de matrículas ativas
                if (cursoExiste.Matriculas.Count(m => m.StatusMatricula == StatusMatricula.Ativa) >= 30)
                {
                    throw new InvalidOperationException("O curso informado já atingiu o limite máximo de 30 matrículas ativas.");
                }

                // Verificar se o aluno já está matriculado em uma matrícula ativa para este curso
                if (cursoExiste.Matriculas.Exists(m => m.IdAluno == idAluno && m.StatusMatricula == StatusMatricula.Ativa))
                {
                    throw new InvalidOperationException("O aluno já está matriculado no curso com uma matrícula ativa.");
                }
            }
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

            // Retornar o Id do matricula recém-inserido
            return matricula.Id;
        }

        private async Task<int> AtualizarMatriculaAsync(Matricula matricula, MatriculaViewModel matriculaViewModel)
        {
            matricula.IdAluno = matriculaViewModel.IdAluno;
            matricula.IdCurso = matriculaViewModel.IdCurso;
            matricula.StatusMatricula = matriculaViewModel.StatusMatricula;

            await _matriculaRepository.AlterarAsync(matricula);

            // Retornar o Id do matricula atualizado
            return matricula.Id;
        }
    }
}
