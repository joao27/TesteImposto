using Imposto.Core.Domain;

namespace Imposto.Core.Service.Contract
{
    public interface INotaFiscalService
    {
        Result GerarNotaFiscal(Pedido pedido);
    }
}
