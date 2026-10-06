using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Models;
using WebApiAutores.Services;


namespace WebApiAutores.Controlllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController(AutorService autorService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AutoresGet>> Create([FromBody] AutoresSet crear)
        {
            int nuevoId = await autorService.SaveAsync(crear);
            var autorCreado = await autorService.GetByIdAsync(nuevoId);
            return CreatedAtAction(nameof(GetById), new { id = nuevoId }, autorCreado);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] AutoresSet actualizar)
        {
            var actualizado = await autorService.UpdateAsync(id, actualizar);
            if (!actualizado)
            {
                return NotFound($"No se encontro el autor con Id {id} para actualizar");
            }

            return Ok(new { message = $"El autor con Id {id} fue actualizado correctamente." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await autorService.DeleteAsync(id);
            if (!eliminado)
            {
                return NotFound(new { message = $"No se encontro el autor con Id {id} para eliminar." });
            }

            return Ok(new { message = $"El autor con Id {id} fue eliminado correctamente." });
        }

        [HttpGet("autores")]
        public async Task<ActionResult<IEnumerable<AutoresGet>>> GetListaAutores()
        {
            var lista = await autorService.GetListAsync();
            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutoresGet>> GetById(int id)
        {
            var autor = await autorService.GetByIdAsync(id);
            if (autor is null)
            {
                return NotFound(new { message = $"No se encontro el autor con ID {id}." });
            }
            return Ok(autor);
        }
    }
}
