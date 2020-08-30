using Imposto.Common.Constant;
using Imposto.Common.Util.Contract;
using Imposto.Core.Data;
using Imposto.Core.Domain;
using Imposto.Core.Service.Contract;
using System;
using System.Collections.Generic;

namespace Imposto.Core.Service.Implementation
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly IXmlWriter<NotaFiscal> _writer;
        private readonly INotaFiscalRepository _notaFiscalRepository;

        public NotaFiscalService(IXmlWriter<NotaFiscal> writer, INotaFiscalRepository notaFiscalRepository)
        {
            _writer = writer;
            _notaFiscalRepository = notaFiscalRepository;
        }

        public Result GerarNotaFiscal(Pedido pedido)
        {
            var notaFiscal = MontarNotaFiscal(pedido);

            var result = _writer.Record(notaFiscal.NumeroNotaFiscal.ToString(), notaFiscal);

            if (result)
            {
                var idNotaFiscal = _notaFiscalRepository.InsertNotaFiscal(notaFiscal.NumeroNotaFiscal, notaFiscal.Serie, 
                        notaFiscal.NomeCliente, notaFiscal.EstadoDestino, notaFiscal.EstadoOrigem);

                foreach (var item in notaFiscal.ItensDaNotaFiscal)
                    _notaFiscalRepository.InsertNotaFiscalItem(idNotaFiscal, item.Cfop, 
                        item.TipoIcms, item.BaseIcms, item.AliquotaIcms, item.ValorIcms, 
                        item.NomeProduto, item.CodigoProduto, item.BaseIpi, item.AliquotaIpi, item.ValorIpi, item.Desconto);
            }

            return new Result() 
            { 
                Status = result, 
                Message = result ? "Operação efetuada com sucesso" : "Erro ao efetuar a operação" 
            };
        }

        private NotaFiscal MontarNotaFiscal(Pedido pedido)
        {
            var notaFiscal = new NotaFiscal
            {
                NumeroNotaFiscal = _notaFiscalRepository.GetNextNumeroNotaFiscal(),
                Serie = new Random().Next(int.MaxValue),
                NomeCliente = pedido.NomeCliente,

                EstadoDestino = pedido.EstadoDestino,
                EstadoOrigem = pedido.EstadoOrigem,
            };

            var desconto = UF.IsSudeste(notaFiscal.EstadoDestino) ? 0.10 : 0.00;
            var cfop = DefinirCfop(notaFiscal.EstadoOrigem, notaFiscal.EstadoDestino);

            foreach (var itemPedido in pedido.ItensDoPedido)
            {
                var notaFiscalItem = new NotaFiscalItem { Cfop = cfop };

                if (notaFiscal.EstadoDestino == notaFiscal.EstadoOrigem)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                }
                else
                {
                    notaFiscalItem.TipoIcms = "10";
                    notaFiscalItem.AliquotaIcms = 0.17;
                }

                if (notaFiscalItem.Cfop == "6.009")
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90;
                else
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;

                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
                notaFiscalItem.AliquotaIpi = 0.10;

                if (itemPedido.Brinde)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                    notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
                    notaFiscalItem.AliquotaIpi = 0.00;
                }

                notaFiscalItem.BaseIpi = itemPedido.ValorItemPedido;
                notaFiscalItem.ValorIpi = notaFiscalItem.BaseIpi * notaFiscalItem.AliquotaIpi;
                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;
                notaFiscalItem.Desconto = desconto;

                notaFiscal.ItensDaNotaFiscal.Add(notaFiscalItem);
            }

            return notaFiscal;
        }

        private static string DefinirCfop(string estadoOrigem, string estadoDestino)
        {
            if (estadoOrigem == UF.SP && estadoDestino == UF.RJ)
                return "6.000";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.PE)
                return "6.001";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.MG)
                return "6.002";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.PB)
                return "6.003";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.PR)
                return "6.004";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.PI)
                return "6.005";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.RO)
                return "6.006";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.SE)
                return "6.007";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.TO)
                return "6.008";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.SE)
                return "6.009";
            else if (estadoOrigem == UF.SP && estadoDestino == UF.PA)
                return "6.010";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.RJ)
                return "6.000";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.PE)
                return "6.001";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.MG)
                return "6.002";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.PB)
                return "6.003";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.PR)
                return "6.004";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.PI)
                return "6.005";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.RO)
                return "6.006";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.SE)
                return "6.007";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.TO)
                return "6.008";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.SE)
                return "6.009";
            else if (estadoOrigem == UF.MG && estadoDestino == UF.PA)
                return "6.010";

            return "0.000";
        }
    }
}
