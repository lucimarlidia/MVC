using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessorController(IProfessorService service)
        {
            _service = service;
        }

        // HTTP POST - Salvar um professor
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProfessorViewModel professorViewModel)
        {
            if (professorViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var professorId = await _service.Salvar(professorViewModel);
                return Ok(new { Message = "Professor salvo com sucesso!", ProfessorId = professorId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar o professor: {ex.Message}");
            }
        }

        // HTTP GET - Obter todos os professores
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var professores = await _service.SelecionarTudo();
                return Ok(professores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os professores: {ex.Message}");
            }
        }

        // HTTP GET - Obter um professor pelo ID
        [HttpGet("{id}")] // id é a query string
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            try
            {
                var professor = await _service.Selecionar(id);

                if (professor == null)
                {
                    return NotFound("Professor não encontrado.");
                }

                return Ok(professor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao selecionar o professor: {ex.Message}");
            }
        }

        // HTTP PUT - Atualizar um professor existente
        [HttpPut("{id}")] // id é a query string
        public async Task<IActionResult> Put(int id, [FromBody] ProfessorViewModel professorViewModel) // o id vai ficar na url e os dados do professor vão ficar no body

        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            if (professorViewModel == null)
            {
                return BadRequest("Dados do professor não fornecidos.");
            }

            if (id != professorViewModel.Id)
            {
                return BadRequest("O ID da URL não corresponde ao ID do professor no corpo da requisição.");
            }

            try
            {
                var professorAtualizado = await _service.Atualizar(professorViewModel);

                if (professorAtualizado == null)
                {
                    return NotFound("Professor não encontrado.");
                }

                return Ok(professorAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o professor: {ex.Message}");
            }
        }

        // HTTP DELETE - Excluir um professor 
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
                    return NotFound("Professor não encontrado.");
                }

                return NoContent(); // Retorna 204 No Content em caso de sucesso
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir o professor: {ex.Message}");
            }
        }
    }

}
