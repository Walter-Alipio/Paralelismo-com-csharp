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

  public async Task<ViewModelDataClient> ProcessarAsync()
  {
    var contas = r_Repositorio.GetContaClientes();

    var contasQuantidadePorThread = contas.Count() / 4;

    var contas_parte1 = contas.Take(contasQuantidadePorThread);
    var contas_parte2 = contas.Skip(contasQuantidadePorThread).Take(contasQuantidadePorThread);
    var contas_parte3 = contas.Skip(contasQuantidadePorThread * 2).Take(contasQuantidadePorThread);
    var contas_parte4 = contas.Skip(contasQuantidadePorThread * 3);

    var resultado = new List<string>();
    _dataClient.tempoDecorrido = $"{TimeSpan.Zero}";

    var inicio = DateTime.Now;

    Thread thread_parte1 = new Thread(() =>
    {
      foreach (var conta in contas_parte1)
      {
        var resultadoProcessamento = r_Servico.ConsolidarMovimentacao(conta);
        _dataClient.Resultado.Add(resultadoProcessamento);
      }
    });
    Thread thread_parte2 = new Thread(() =>
    {
      foreach (var conta in contas_parte2)
      {
        var resultadoProcessamento = r_Servico.ConsolidarMovimentacao(conta);
        _dataClient.Resultado.Add(resultadoProcessamento);
      }
    });
    Thread thread_parte3 = new Thread(() =>
    {
      foreach (var conta in contas_parte3)
      {
        var resultadoProcessamento = r_Servico.ConsolidarMovimentacao(conta);
        _dataClient.Resultado.Add(resultadoProcessamento);
      }
    });
    Thread thread_parte4 = new Thread(() =>
    {
      foreach (var conta in contas_parte4)
      {
        var resultadoProcessamento = r_Servico.ConsolidarMovimentacao(conta);
        _dataClient.Resultado.Add(resultadoProcessamento);
      }
    });

    thread_parte1.Start();
    thread_parte2.Start();
    thread_parte3.Start();
    thread_parte4.Start();

    while (thread_parte1.IsAlive || thread_parte2.IsAlive
        || thread_parte3.IsAlive || thread_parte4.IsAlive)
    {
      Thread.Sleep(250);
      //Não vou fazer nada
    }

    var fim = DateTime.Now;

    TimeSpan elapsedTime = fim - inicio;
    _dataClient.tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
    _dataClient.mensagem = $"Processamento de {_dataClient.Resultado.Count} clientes em {_dataClient.tempoDecorrido}";



    return _dataClient;
  }

  // private void AtualizarView(List<String> result, TimeSpan elapsedTime)
  // {
  //   var tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
  //   var mensagem = $"Processamento de {result.Count} clientes em {tempoDecorrido}";

  //   LstResultados.ItemsSource = result;
  //   TxtTempo.Text = mensagem;
  // }
}