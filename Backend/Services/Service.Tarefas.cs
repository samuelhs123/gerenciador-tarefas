using GerenciadorTarefas.Data.Models;
using GerenciadorTarefas.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GerenciadorTarefas.Services;

public partial class GerenciadorTarefasService
{
    internal async Task<RespostaAPI> GetTarefas(List<TarefaSituacaoEnum> situacoes, TiposOrdenacaoTarefas ordenacao)
    {
        if (situacoes.IsNullOrEmpty())
        {
            return new(new List<Tarefa>());
        }

        try
        {
            IQueryable<Tarefa> query = _gerenciadorTarefasDbContext.Tarefas.Where(t => situacoes.Contains(t.Situacao));

            switch (ordenacao)
            {
                case TiposOrdenacaoTarefas.Titulo:
                {
                    query = query.OrderBy(t => t.Titulo)
                        .ThenBy(t => t.DataConclusaoPrevista);
                    break;
                }

                case TiposOrdenacaoTarefas.Situacao:
                {
                    query = query.OrderBy(t => t.Situacao)
                        .ThenBy(t => t.DataConclusaoPrevista);
                    break;
                }

                case TiposOrdenacaoTarefas.DataConclusaoPrevista:
                {
                    query = query.OrderBy(t => t.DataConclusaoPrevista)
                        .ThenBy(t => t.Titulo);
                    break;
                }

                case TiposOrdenacaoTarefas.DataConclusaoEfetiva:
                {
                    query = query.OrderBy(t => t.DataConclusaoEfetiva)
                        .ThenBy(t => t.Titulo);
                    break;
                }
            }

            return new(await query.ToArrayAsync());
        }
        catch (Exception ex)
        {
            return new RespostaAPI(false, ex.Message);
        }
    }

    internal async Task<RespostaAPI> GetTarefa(int id)
    {
        var tarefa = await _gerenciadorTarefasDbContext.Tarefas.FindAsync(id);
        if (tarefa == null)
        {
            return new(false, "Tarefa não encontrada");
        }

        return new(tarefa);
    }

    internal async Task<RespostaAPI> PostTarefa(Tarefa tarefa)
    {
        var respValidacao = ValidarTarefa(tarefa);
        if (!respValidacao.Sucesso)
        {
            return respValidacao;
        }

        await _gerenciadorTarefasDbContext.AddAsync(tarefa);
        await _gerenciadorTarefasDbContext.SaveChangesAsync(true);
        return new(tarefa);
    }

    internal async Task<RespostaAPI> DeleteTarefa(int id)
    {
        var tarefa = await _gerenciadorTarefasDbContext.Tarefas.FindAsync(id);
        if (tarefa == null)
        { 
            return new(false, "Tarefa não encontrada");
        }

        _gerenciadorTarefasDbContext.Tarefas.Remove(tarefa);
        await _gerenciadorTarefasDbContext.SaveChangesAsync();
        return new(true);
    }

    internal async Task<RespostaAPI> PutTarefa(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id)
        { 
            return new RespostaAPI(false, "O ID da tarefa informado não confere");
        }

        var oldTarefa = await _gerenciadorTarefasDbContext.Tarefas.FindAsync(id);
        if (oldTarefa == null)
        { 
            return new(false, "Tarefa não encontrada");
        }

        if (tarefa.Situacao == TarefaSituacaoEnum.Pendente)
        {
            tarefa.DataConclusaoEfetiva = null;
        }
        else if (tarefa.Situacao == TarefaSituacaoEnum.Concluida)
        {
            tarefa.DataConclusaoEfetiva = DateTime.Now;
        }

        var respValidacao = ValidarTarefa(tarefa);
        if (!respValidacao.Sucesso)
        {
            return respValidacao;
        }

        _gerenciadorTarefasDbContext.Attach(oldTarefa);
        _gerenciadorTarefasDbContext.Entry(oldTarefa).CurrentValues.SetValues(tarefa);
        await _gerenciadorTarefasDbContext.SaveChangesAsync();
        
        return new(tarefa);
    }

    private RespostaAPI ValidarTarefa(Tarefa tarefa)
    {
        if (string.IsNullOrWhiteSpace(tarefa.Titulo))
        {
            return new RespostaAPI(false, "O título da tarefa deve ser preenchido");
        }

        if (string.IsNullOrWhiteSpace(tarefa.Descricao))
        {
            return new RespostaAPI(false, "A descrição da tarefa deve ser preenchida");
        }

        if (tarefa.Situacao == TarefaSituacaoEnum.Pendente)
        {
            if (!tarefa.DataConclusaoPrevista.HasValue)
            {
                return new RespostaAPI(false, "A data prevista para conclusão da tarefa deve ser informada");
            }

            if (tarefa.DataConclusaoPrevista.Value.Date < DateTime.Today)
            {
                return new RespostaAPI(false, "A data prevista para conclusão da tarefa não deve ser anterior à data atual");
            }
        }
        else if (tarefa.Situacao == TarefaSituacaoEnum.Concluida)
        {
            if (!tarefa.DataConclusaoEfetiva.HasValue)
            {
                return new RespostaAPI(false, "A data efetiva de conclusão da tarefa deve ser informada");
            }

            if (tarefa.DataConclusaoEfetiva.Value.Date > DateTime.Today)
            {
                return new RespostaAPI(false, "A data efetiva de conclusão da tarefa não deve ser posterior à data atual");
            }
        }

        return new RespostaAPI(true);
    }
}

public enum TiposOrdenacaoTarefas
{
    Situacao = 0,
    Titulo = 1,
    DataConclusaoPrevista = 2,
    DataConclusaoEfetiva = 3,
}