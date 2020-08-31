using FakeItEasy;
using Imposto.Common.Util.Contract;
using Imposto.Core.Data;
using Imposto.Core.Domain;
using Imposto.Core.Service.Contract;
using Imposto.Core.Service.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imposto.Test
{
    [TestClass]
    public class NotaFiscalServiceTest
    {
        private readonly IXmlWriter<NotaFiscal> _xmlWriter;
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly INotaFiscalService _notaFiscalService;

        public NotaFiscalServiceTest()
        {
            _xmlWriter = A.Fake<IXmlWriter<NotaFiscal>>();
            _notaFiscalRepository = A.Fake<INotaFiscalRepository>();
            _notaFiscalService = A.Fake<INotaFiscalService>();
        }

        [TestMethod]
        public void GerarNotaFiscal_HavingGeneratedAValidXml_ShouldPersistNotaFiscalInDataBase()
        {
            // arrange
            var pedido = A.Dummy<Pedido>();
            pedido.ItensDoPedido.Add(A.Dummy<PedidoItem>());

            A.CallTo(() => _xmlWriter.Record(A<string>.Ignored, A<NotaFiscal>.Ignored)).Returns(true);

            A.CallTo(() => _notaFiscalRepository.GetNextIdNotaFiscal()).Returns(A.Dummy<int>());

            A.CallTo(() => _notaFiscalRepository.GetLastIdNotaFiscalItem()).Returns(A.Dummy<int>());

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscal(A<int>.Ignored, A<int>.Ignored, 
                A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(1);

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscalItem(A<int>.Ignored, A<string>.Ignored, A<string>.Ignored, 
                A<double>.Ignored, A<double>.Ignored, A<double>.Ignored, A<string>.Ignored, A<string>.Ignored, A<double>.Ignored, A<double>.Ignored, A<double>.Ignored, A<double>.Ignored));

            // act
            var notaFiscalService = new NotaFiscalService(_xmlWriter, _notaFiscalRepository);
            var result = notaFiscalService.GerarNotaFiscal(pedido);

            // assert
            A.CallTo(() => _xmlWriter.Record(A<string>.Ignored, A<NotaFiscal>.Ignored))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.GetNextIdNotaFiscal()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.GetLastIdNotaFiscalItem()).MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscal(A<int>.Ignored, A<int>.Ignored,
                A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscalItem(A<int>.Ignored, A<string>.Ignored, A<string>.Ignored,
                A<double>.Ignored, A<double>.Ignored, A<double>.Ignored, A<string>.Ignored, A<string>.Ignored, A<double>.Ignored,
                A<double>.Ignored, A<double>.Ignored, A<double>.Ignored)).MustHaveHappenedOnceOrMore();

            Assert.IsTrue(result.Status);
        }

        [TestMethod]
        public void GerarNotaFiscal_HavingGeneratedAnInvalidXml_ShouldNotPersistNotaFiscalInDataBase()
        {
            // arrange
            var pedido = A.Dummy<Pedido>();
            pedido.ItensDoPedido.Add(A.Dummy<PedidoItem>());

            A.CallTo(() => _xmlWriter.Record(A<string>.Ignored, A<NotaFiscal>.Ignored)).Returns(false);

            A.CallTo(() => _notaFiscalRepository.GetNextIdNotaFiscal()).Returns(A.Dummy<int>());

            A.CallTo(() => _notaFiscalRepository.GetLastIdNotaFiscalItem()).Returns(A.Dummy<int>());

            // act
            var notaFiscalService = new NotaFiscalService(_xmlWriter, _notaFiscalRepository);
            var result = notaFiscalService.GerarNotaFiscal(pedido);

            // assert
            A.CallTo(() => _xmlWriter.Record(A<string>.Ignored, A<NotaFiscal>.Ignored))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.GetNextIdNotaFiscal())
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.GetLastIdNotaFiscalItem())
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscal(A<int>.Ignored, A<int>.Ignored,
                A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustNotHaveHappened();

            A.CallTo(() => _notaFiscalRepository.InsertNotaFiscalItem(A<int>.Ignored, A<string>.Ignored, A<string>.Ignored,
                A<double>.Ignored, A<double>.Ignored, A<double>.Ignored, A<string>.Ignored, A<string>.Ignored, A<double>.Ignored, 
                A<double>.Ignored, A<double>.Ignored, A<double>.Ignored)).MustNotHaveHappened();

            Assert.IsFalse(result.Status);
        }
    }
}
