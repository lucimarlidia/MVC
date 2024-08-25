using Microsoft.AspNetCore.Mvc;
using MVC.Web.Models.ViewModels;
using MVC.Web.Services;

namespace MVC.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoService _service;

        public CursoController(ICursoService service)
        {
            _service = service;
        }

        // HTTP POST - Salvar um curso
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CursoViewModel cursoViewModel)
        {
            if (cursoViewModel == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var cursoId = await _service.Salvar(cursoViewModel);
                return Ok(new { Message = "Curso salvo com sucesso!", CursoId = cursoId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar o curso: {ex.Message}");
            }
        }

        // HTTP GET - Obter todos os cursos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var Cursos = await _service.SelecionarTudo();
                return Ok(Cursos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar os Cursos: {ex.Message}");
            }
        }

        // HTTP GET - Obter um curso pelo ID
        [HttpGet("{id}")] // id é a query string
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var Curso = await _service.Selecionar(id);

                if (Curso == null)
                {
                    return NotFound("Curso não encontrado.");
                }

                return Ok(Curso);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao selecionar o Curso: {ex.Message}");
            }
        }

        // HTTP PUT - Atualizar um curso existente
        [HttpPut("{id}")] // id é a query string
        public async Task<IActionResult> Put(int id, [FromBody] CursoViewModel CursoViewModel) // o id vai ficar na url e os dados do curso vão ficar no body
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            if (CursoViewModel == null)
            {
                return BadRequest("Dados do Curso inválidos.");
            }

            if (id != CursoViewModel.Id)
            {
                return BadRequest("O ID da URL é diferente do ID do Curso no corpo da requisição.");
            }

            try
            {
                var CursoAtualizado = await _service.Atualizar(CursoViewModel);

                if (CursoAtualizado == null)
                {
                    return NotFound("Curso não encontrado.");
                }

                return Ok(CursoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o Curso: {ex.Message}");
            }
        }

        // HTTP DELETE - Excluir um curso
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
                    return NotFound("Curso não encontrado.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir o Curso: {ex.Message}");
            }
        }
    }
}

