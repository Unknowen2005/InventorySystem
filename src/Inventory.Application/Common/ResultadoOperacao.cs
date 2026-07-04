namespace Inventory.Application.Common;

public class ResultadoOperacao
{
    public bool Sucesso { get; private set; }
    public string Mensagem { get; private set; }
    public object Dados { get; private set; }

    private ResultadoOperacao(bool sucesso, string mensagem, object dados = null)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
        Dados = dados;
    }

    public static ResultadoOperacao Ok(string mensagem = "Operação realizada com sucesso.", object dados = null)
        => new ResultadoOperacao(true, mensagem, dados);

    public static ResultadoOperacao Falha(string mensagem, object dados = null)
        => new ResultadoOperacao(false, mensagem, dados);
}