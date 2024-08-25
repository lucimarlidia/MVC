using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _service;

        public AlunoController(IAlunoService service)
        {
            _service = service;
        }

        // HTTP POST - Salvar um aluno
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlunoViewModel alunoViewModel)
        {
            if (alunoViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var alunoId = await _service.Salvar(alunoViewModel);
                return Ok(new { Message = "Aluno salvo com sucesso!", AlunoId = alunoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar o aluno: {ex.Message}");
            }
        }

        // HTTP GET - Obter todos os alunos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var alunos = await _service.SelecionarTudo();
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao selecionar os alunos: {ex.Message}");
            }
        }

        // HTTP GET - Obter um aluno pelo ID
        [HttpGet("{id}")] // id é a query string
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var aluno = await _service.Selecionar(id);

                if (aluno == null)
                {
                    return NotFound("Aluno não encontrado.");
                }

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao selecionar o aluno: {ex.Message}");
            }
        }

        // HTTP PUT - Atualizar um aluno existente
        [HttpPut("{id}")] // id é a query string
        public async Task<IActionResult> Put(int id, [FromBody] AlunoViewModel alunoViewModel) // o id vai ficar na url e os dados do aluno vão ficar no body
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            if (alunoViewModel == null)
            {
                return BadRequest("Dados do aluno inválidos.");
            }

            if (id != alunoViewModel.Id)
            {
                return BadRequest("O ID da URL é diferente do ID do aluno no corpo da requisição.");
            }

            try
            {
                var alunoAtualizado = await _service.Atualizar(alunoViewModel);

                if (alunoAtualizado == null)
                {
                    return NotFound("Aluno não encontrado.");
                }

                return Ok(alunoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o aluno: {ex.Message}");
            }
        }

        // HTTP DELETE - Excluir um aluno
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
                    return NotFound("Aluno não encontrado.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir o aluno: {ex.Message}");
            }
        }
    }
}
