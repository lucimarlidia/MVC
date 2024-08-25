using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaService _service;

        public MatriculaController(IMatriculaService service)
        {
            _service = service;
        }

        // HTTP POST - Salvar uma matrícula
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MatriculaViewModel matriculaViewModel)
        {
            if (matriculaViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var matriculaId = await _service.Salvar(matriculaViewModel);
                return Ok(new { Message = "Matrícula salva com sucesso!", MatriculaId = matriculaId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar a matrícula: {ex.Message}");
            }
        }

        // HTTP GET - Obter todas as matrículas
        [HttpGet]
        public async Task<IActionResult> GetTodas()
        {
            try
            {
                var matriculas = await _service.SelecionarTudo();
                return Ok(matriculas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar as matrículas: {ex.Message}");
            }
        }

        // HTTP GET - Obter uma matrícula pelo ID
        [HttpGet("{id}")] // id é a query string
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var matricula = await _service.Selecionar(id);

                if (matricula == null)
                {
                    return NotFound("Matrícula não encontrada.");
                }

                return Ok(matricula);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao selecionar a matrícula: {ex.Message}");
            }
        }

        // HTTP PUT - Atualizar uma matrícula existente
        [HttpPut("{id}")] // id é a query string
        public async Task<IActionResult> Put(int id, [FromBody] MatriculaViewModel matriculaViewModel) // o id vai ficar na url e os dados da matrícula vão ficar no body

        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            if (matriculaViewModel == null)
            {
                return BadRequest("Dados da matrícula não fornecidos.");
            }

            if (id != matriculaViewModel.Id)
            {
                return BadRequest("O ID da URL é diferente do ID do aluno no corpo da requisição.");
            }

            try
            {
                var matriculaAtualizada = await _service.Atualizar(matriculaViewModel);

                if (matriculaAtualizada == null)
                {
                    return NotFound("Matrícula não encontrada.");
                }

                return Ok(matriculaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a matrícula: {ex.Message}");
            }
        }

        // HTTP DELETE - Excluir uma matrícula 
        [HttpDelete("{id}")] // id é a query string
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var sucesso = await _service.Excluir(id);

                if (!sucesso)
                {
                    return NotFound("Matrícula não encontrada.");
                }

                return NoContent(); // Retorna 204 No Content em caso de sucesso
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir a matrícula: {ex.Message}");
            }
        }
    }
}
