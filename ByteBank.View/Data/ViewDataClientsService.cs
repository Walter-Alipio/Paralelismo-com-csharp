using ByteBank.Core.Model;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;
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

  public async Task<ViewModelDataClient> ProcessarAsync()
  {
    var contas = r_Repositorio.GetContaClientes();

    var resultado = new List<string>();
    _dataClient.tempoDecorrido = $"{TimeSpan.Zero}";

    var inicio = DateTime.Now;

    _dataClient.Resultado = new List<string>(await ConsolidarContas(contas));

    var fim = DateTime.Now;
    TimeSpan elapsedTime = fim - inicio;
    _dataClient.tempoDecorrido = $"{elapsedTime.Seconds}.{elapsedTime.Milliseconds} segundos!";
    _dataClient.mensagem = $"Processamento de {_dataClient.Resultado.Count} clientes em {_dataClient.tempoDecorrido}";


    return _dataClient;
  }

  private async Task<string[]> ConsolidarContas(IEnumerable<ContaCliente> contas)
  {
    var tasks = contas.Select(conta =>
       Task.Factory.StartNew(() => r_Servico.ConsolidarMovimentacao(conta))
    );

    return await Task.WhenAll(tasks);
  }

}