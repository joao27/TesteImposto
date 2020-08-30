namespace Imposto.Core.Data
{
    public interface INotaFiscalRepository
    {
        int GetNextNumeroNotaFiscal();

        int InsertNotaFiscal(int numeroNotaFiscal, int serie, string nomeCliente, string estadoDestino, string estadoOrigem);

        void InsertNotaFiscalItem(int idNotaFiscal, string cfop, string tipoIcms, double baseIcms, double aliquotaIcms, double valorIcms, string nomeProduto, string codigoProduto, double baseIpi, double aliquotaIpi, double valorIpi, double desconto);
    }
}
