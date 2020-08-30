using Imposto.Common.Constant;
using Imposto.Core.Domain;
using Imposto.Core.Service;
using Imposto.Core.Service.Contract;
using Imposto.Core.Service.Implementation;
using System;
using System.Data;
using System.Windows.Forms;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private readonly INotaFiscalService _notaFiscalService;

        public FormImposto(INotaFiscalService notaFiscalService)
        {
            InitializeComponent();

            InitializeFields();

            ResizeColumns();

            _notaFiscalService = notaFiscalService;
        }

        private void InitializeFields()
        {
            textBoxNomeCliente.Text = string.Empty;

            dataGridViewPedidos.AutoGenerateColumns = true;
            dataGridViewPedidos.DataSource = GetTablePedidos();

            cbEstadoOrigem.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEstadoOrigem.Items.Clear();
            cbEstadoOrigem.Items.AddRange(UF.GetUfsOrigem());

            cbEstadoDestino.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEstadoDestino.Items.Clear();
            cbEstadoDestino.Items.AddRange(UF.GetUfsDestino());
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }   
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(object)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)) { DefaultValue = false });
                     
            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            var table = (DataTable)dataGridViewPedidos.DataSource;

            var pedido = new Pedido()
            {
                EstadoOrigem = cbEstadoOrigem.Text,
                EstadoDestino = cbEstadoDestino.Text,
                NomeCliente = textBoxNomeCliente.Text
            };

            var result = new Result() { Status = true, Message = string.Empty };

            if (table.Rows.Count == 0)
            {
                result.Status = false;
                result.Message = "Por favor, adicione ao menos um item no pedido";
            }

            foreach (DataRow row in table.Rows)
            {
                var validValor = double.TryParse(row["Valor"].ToString(), out double valor);
                var validLenghtNomeProduto = row["Nome do produto"].ToString().Length <= 50;
                var validLenghtCodigoProduto = row["Codigo do produto"].ToString().Length <= 20;

                ValidarInputs(result, validValor, validLenghtNomeProduto, validLenghtCodigoProduto);

                if (!result.Status)
                    break;

                pedido.ItensDoPedido.Add(
                    new PedidoItem()
                    {
                        Brinde = Convert.ToBoolean(row["Brinde"]),
                        CodigoProduto =  row["Codigo do produto"].ToString(),
                        NomeProduto = row["Nome do produto"].ToString(),
                        ValorItemPedido = valor         
                    });
            }

            if (result.Status)
            {
                result = _notaFiscalService.GerarNotaFiscal(pedido); 
                InitializeFields();
            }

            MessageBox.Show(result.Message, "", MessageBoxButtons.OK, result.Status ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void ValidarInputs(Result result, bool validValor, bool lenghtNomeProduto, bool lenghtCodigoProduto)
        {
            if (!validValor) result.Message += "Há valores inválidos nos itens do pedido. \n \n";

            if (!lenghtNomeProduto) result.Message += "O tamanho máximo do Nome do Produto é de 50 caracteres. \n \n";

            if (!lenghtCodigoProduto) result.Message += "O tamanho máximo do Código do Produto é de 20 caracteres. \n \n";

            if (!validValor || !lenghtNomeProduto || !lenghtCodigoProduto) result.Status = false;
        }
    }
}
