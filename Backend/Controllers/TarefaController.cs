using GerenciadorTarefas.Data.Models;
using GerenciadorTarefas.Services;
using GerenciadorTarefas.Util;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TarefasController(GerenciadorTarefasService gerenciadorTarefasService) : ControllerBase
{
    private readonly GerenciadorTarefasService _gerenciadorTarefasService = gerenciadorTarefasService;

    [HttpGet]
    public async Task<ActionResult<RespostaAPI>> Get([FromQuery] List<TarefaSituacaoEnum> situacoes, [FromQuery] TiposOrdenacaoTarefas ordenacao)
    {
        return Ok(await _gerenciadorTarefasService.GetTarefas(situacoes, ordenacao));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RespostaAPI>> Get(int id)
    {
        return Ok(await _gerenciadorTarefasService.GetTarefa(id));
    }

    [HttpPost]
    public async Task<ActionResult<RespostaAPI>> Post(Tarefa tarefa) 
    {
        return Ok(await _gerenciadorTarefasService.PostTarefa(tarefa));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RespostaAPI>> Put(int id, Tarefa tarefa)
    {
        return Ok(await _gerenciadorTarefasService.PutTarefa(id, tarefa));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RespostaAPI>> Delete(int id)
    {
        return Ok(await _gerenciadorTarefasService.DeleteTarefa(id));
    }
}