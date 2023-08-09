using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using TodoWinForm.Classes;
using System.Linq;

namespace TodoWinForm
{
    public partial class frmMain : Form
    {
        List<TodoItem> todoItems = new List<TodoItem>();
        ConvertListToDataTable<TodoItem> ConvertItemData = new ConvertListToDataTable<TodoItem>();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TodoItem item = new TodoItem()
                {
                    Name = txtTodo.Text,
                    IsCompleted = cbbCompleted.SelectedIndex == 0 ? true : false
                };

                string json = JsonConvert.SerializeObject(item, Formatting.None);

                item.Id = new BoTodoItem().InsereTodoItem(item);

                CarregaDgvTodoItem();
                MessageBox.Show("Tarefa incluída com sucesso.\nId: " + item.Id, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                TodoItem itemSelecionado = todoItems.Find(i => i.Id == (long)dgvTodoItems.CurrentRow.Cells[0].Value);
                string mensagem = new BoTodoItem().ExcluiTodoItem(itemSelecionado);

                CarregaDgvTodoItem();
                MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                CarregaDgvTodoItem();
                //Se houver algum item selecionado no dataGridView, o botão para deleção é habilitado.
                btnDel.Enabled = VerificaItemSelDgv();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void dgvTodoItems_DoubleClick(object sender, EventArgs e)
        {
            if (VerificaItemSelDgv())
            {
                TodoItem itemSelecionado = todoItems.Find(i => i.Id == (long)dgvTodoItems.CurrentRow.Cells[0].Value);

                frmEdit update = new frmEdit(itemSelecionado);
                update.ShowDialog();
                CarregaDgvTodoItem();
            }
        }

        //Métodos para carregamento, formatação e verificação de linha selecionada.
        private void FormataDgvTodoItem()
        {
            dgvTodoItems.Columns[0].Visible = false;
        }
        private void CarregaDgvTodoItem()
        {
            try
            {
                todoItems = new BoTodoItem().BuscarTodoItems();

                //ConvertItemData é um objeto do tipo ConvertListToDataTable (Classe abaixo)
                dgvTodoItems.DataSource = ConvertItemData.ConvertToDataTable(todoItems);
                FormataDgvTodoItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        private bool VerificaItemSelDgv()
        {
            if (dgvTodoItems.Rows.Count > 0)
            {
                Int32 selectedRowCount = dgvTodoItems.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private void dgvTodoItems_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            btnDel.Enabled = VerificaItemSelDgv();
        }

    }

    //Classe para converter um List<T> para DataTable, deste modo, um jeito simples de incluir num DataSource
    public class ConvertListToDataTable<T>
    {
        public DataTable ConvertToDataTable(List<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
