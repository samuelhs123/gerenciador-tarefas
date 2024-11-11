namespace GerenciadorTarefas.Util;

public class RespostaAPI
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public object Retorno { get; set; }

    public RespostaAPI(bool sucesso, string mensagem = null)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
    }

    public RespostaAPI(object retorno)
    {
        Sucesso = true;
        Retorno = retorno;
    }
}