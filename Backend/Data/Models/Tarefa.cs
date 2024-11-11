using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.Data.Models;

public class Tarefa
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Titulo { get; set; }

    [MaxLength(500)]
    public string Descricao { get; set; }
    public DateTime? DataConclusaoPrevista { get; set; }
    public DateTime? DataConclusaoEfetiva { get; set; }
    public TarefaSituacaoEnum Situacao { get; set; } = TarefaSituacaoEnum.Pendente;
}

public enum TarefaSituacaoEnum
{
    Pendente = 0,
    Concluida = 1,
}