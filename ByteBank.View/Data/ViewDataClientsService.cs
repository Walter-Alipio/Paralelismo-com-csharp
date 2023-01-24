using ByteBank.Core.Model;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;
using ByteBank.View.Utils;

namespace ByteBank.View.Data;
public class ViewDataClientsService
{
  private readonly ContaClienteRepository r_Repositorio;
  private readonly ContaClienteService r_Servico;
  private ViewModelDataClient _dataClient;

  public ViewDataClientsService()
  {
    r_Repositorio = new ContaClienteRepository();
    r_Servico = new ContaClienteService();
    _dataClient = new ViewModelDataClient();
  }

  public async Task<ViewModelDataClient> ProcessarAsync(CancellationToken ct, IProgress<string> progress)
  {
    var contas = r_Repositorio.GetContaClientes();

    var resultado = new List<string>();
    _dataClient.tempoDecorrido = $"{TimeSpan.Zero}";

    var inicio = DateTime.Now;

    try
    {
      _dataClient.Resultado = new List<string>(await ConsolidarContas(contas, ct, progress));

      var fim = DateTime.Now;
      TimeSpan elapsedTime = fim - inicio;
      _dataClient.tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
      _dataClient.mensagem = $"Processamento de {_dataClient.Resultado.Count} clientes em {_dataClient.tempoDecorrido}";
    }
    catch (OperationCanceledException)
    {
      _dataClient.Resultado = new List<string>();
      _dataClient.mensagem = $"Processamento cancelado pelo usuário";
    }



    return _dataClient;
  }

  private async Task<string[]> ConsolidarContas(IEnumerable<ContaCliente> contas, CancellationToken ct, IProgress<string> progress)
  {
    var tasks = contas.Select(conta =>
       Task.Factory.StartNew(() =>
        {
          ct.ThrowIfCancellationRequested();

          var resultadoConsolidacao = r_Servico.ConsolidarMovimentacao(conta, ct);

          progress.Report(resultadoConsolidacao);

          ct.ThrowIfCancellationRequested();

          return resultadoConsolidacao;
        })
    );

    return await Task.WhenAll(tasks);
  }

  public int GetTotalOfContas()
  {
    return r_Repositorio.GetContaClientes().Count();
  }
}