﻿using MarcenariaApi.Data;
using MarcenariaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarcenariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private MarcenariaDbContext _dbContext;
        public TarefaController(MarcenariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> Listar()
        {
            if (_dbContext is null) return NotFound();
            if (_dbContext.Tarefas is null) return NotFound();
            return await _dbContext.Tarefas.Include(e => e.Materiais).ToListAsync();
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<Tarefa>> Buscar(int id)
        {
            if (_dbContext is null) return NotFound();
            if (_dbContext.Tarefas is null) return NotFound();
            var tarefaTemp = await _dbContext.Tarefas.Include(e => e.Materiais).FirstOrDefaultAsync(e => e.id == id);
            if (tarefaTemp is null) return NotFound();
            return tarefaTemp;
        }

        [HttpPost]
        [Route("cadastrar")]
        public async Task<ActionResult> Cadastrar(Tarefa tarefa)
        {
            if (_dbContext is null) return NotFound();
            if (_dbContext.Tarefas is null) return NotFound();
            if (_dbContext.Projetos is null) return NotFound("Não há projetos cadastrados!");

            var projetoTemp = await _dbContext.Projetos.FindAsync(tarefa.ProjetoId);
            if (projetoTemp is null) return NotFound("Projeto não encontrado!");

            await _dbContext.AddAsync(tarefa);
            await _dbContext.SaveChangesAsync();
            return Created("Tarefa cadastrada com sucesso!", tarefa);
        }

        [HttpPut()]
        [Route("atualizar")]
        public async Task<ActionResult> Alterar(Tarefa tarefa)
        {
            if (_dbContext is null) return NotFound();
            if (_dbContext.Tarefas is null) return NotFound();
            var tarefaTemp = await _dbContext.Tarefas.FindAsync(tarefa.id);
            if (tarefaTemp is null) return NotFound();
            _dbContext.Tarefas.Update(tarefa);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete()]
        [Route("deletar/{id}")]
        public async Task<ActionResult> Excluir(int id)
        {
            if (_dbContext is null) return NotFound();
            if (_dbContext.Tarefas is null) return NotFound();
            var tarefaTemp = await _dbContext.Tarefas.FindAsync(id);
            if (tarefaTemp is null) return NotFound();
            _dbContext.Remove(tarefaTemp);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }

}
