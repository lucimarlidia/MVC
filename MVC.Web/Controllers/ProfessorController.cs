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
    }
}
