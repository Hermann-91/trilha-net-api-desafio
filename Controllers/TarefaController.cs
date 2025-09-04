using System.Collections.Immutable;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefas = _context.Tarefas.Find(id); //OK TODO: Buscar o Id no banco utilizando o EF
            if (tarefas == null)
                return NotFound("Não temos esse Id"); //OK TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            
            return Ok(tarefas); //OK caso contrário retornar OK com a tarefa encontrada
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var obterTodos = _context.Tarefas.ToList();// OK TODO: Buscar todas as tarefas no banco utilizando o EF
            return Ok(obterTodos);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)); // OK TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            if (tarefas == null)                                                                      // Dica: Usar como exemplo o endpoint ObterPorData
            return NotFound("não encontramos essa titulo");

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
           
            
            var tarefa = _context.Tarefas.Where(x => x.Status == status);  // OK TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            return Ok(tarefa);                                              // Dica: Usar como exemplo o endpoint ObterPorData
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();// OK TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Data = tarefa.Data;// OK TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Status = tarefa.Status;
            _context.Tarefas.Update(tarefaBanco); // OK TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.SaveChanges();
           
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();// OK TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)

            return NoContent();
        }
    }
}
