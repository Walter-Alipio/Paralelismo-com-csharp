using ByteBank.Core.Repository;
using ByteBank.Core.Service;

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

  public Task<ViewModelDataClient> ProcessarAsync()
  {
    var taskSchedulerUI = TaskScheduler.FromCurrentSynchronizationContext();
    var contas = r_Repositorio.GetContaClientes();

    var resultado = new List<string>();
    _dataClient.tempoDecorrido = $"{TimeSpan.Zero}";

    var inicio = DateTime.Now;

    var contasTarefas = contas.Select(conta =>
    {
      return Task.Factory.StartNew(() =>
      {
        var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
        _dataClient.Resultado.Add(resultadoConta);
      });
    }).ToArray();


    Task.WaitAll(contasTarefas);
    var fim = DateTime.Now;

    TimeSpan elapsedTime = fim - inicio;
    _dataClient.tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
    _dataClient.mensagem = $"Processamento de {_dataClient.Resultado.Count} clientes em {_dataClient.tempoDecorrido}";

    // Task.WhenAll(contasTarefas)
    //   .ContinueWith(contas =>
    //   {
    //     var fim = DateTime.Now;

    //     TimeSpan elapsedTime = fim - inicio;
    //     _dataClient.tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
    //     _dataClient.mensagem = $"Processamento de {_dataClient.Resultado.Count} clientes em {_dataClient.tempoDecorrido}";
    //   }, taskSchedulerUI);


    return Task.FromResult(_dataClient);
  }
}