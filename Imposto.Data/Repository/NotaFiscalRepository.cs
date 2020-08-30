using Dapper;
using Imposto.Core.Data;
using Imposto.Data.Transaction;
using System.Data;
using System.Linq;

namespace Imposto.Data.Repository
{
    public class NotaFiscalRepository : RepositoryBase, INotaFiscalRepository
    {
        public NotaFiscalRepository(IContext context) : base(context) { }

        public int InsertNotaFiscal(int numeroNotaFiscal, int serie, string nomeCliente, string estadoDestino, string estadoOrigem)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@pId", 0, direction: ParameterDirection.InputOutput);
            parameters.Add("@pNumeroNotaFiscal", numeroNotaFiscal);
            parameters.Add("@pSerie", serie);
            parameters.Add("@pNomeCliente", nomeCliente);
            parameters.Add("@pEstadoDestino", estadoDestino);
            parameters.Add("@pEstadoOrigem", estadoOrigem);

            ExecuteProcedure("[P_NOTA_FISCAL]", parameters);

            return parameters.Get<int>("@pId");
        }

        public void InsertNotaFiscalItem(
                int idNotaFiscal, 
                string cfop, 
                string tipoIcms, 
                double baseIcms,
                double aliquotaIcms,
                double valorIcms, 
                string nomeProduto, 
                string codigoProduto,
                double baseIpi,
                double aliquotaIpi,
                double valorIpi,
                double desconto
            )
        {
            var parameters = new DynamicParameters();

            parameters.Add("@pId", 0);
            parameters.Add("@pIdNotaFiscal", idNotaFiscal);
            parameters.Add("@pCfop", cfop);
            parameters.Add("@pTipoIcms", tipoIcms);
            parameters.Add("@pBaseIcms", baseIcms);
            parameters.Add("@pAliquotaIcms", aliquotaIcms);
            parameters.Add("@pValorIcms", valorIcms);
            parameters.Add("@pNomeProduto", nomeProduto);
            parameters.Add("@pCodigoProduto", codigoProduto);
            parameters.Add("@pBaseIpi", baseIpi);
            parameters.Add("@pAliquotaIpi", aliquotaIpi);
            parameters.Add("@pValorIpi", valorIpi);
            parameters.Add("@pDesconto", desconto);

            ExecuteProcedure("[P_NOTA_FISCAL_ITEM]", parameters);
        }

        public int GetNextNumeroNotaFiscal() => Query<int>("SELECT TOP 1 (NUMERONOTAFISCAL + 1) FROM NOTAFISCAL ORDER BY ID DESC");
    }
}
