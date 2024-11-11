using GerenciadorTarefas.Data;

namespace GerenciadorTarefas.Services;

public partial class GerenciadorTarefasService(GerenciadorTarefasDbContext gerenciadorTarefasDbContext)
{
    private readonly GerenciadorTarefasDbContext _gerenciadorTarefasDbContext = gerenciadorTarefasDbContext;
}