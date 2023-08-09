using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using TodoWinForm.Classes;

namespace TodoWinForm
{
    public partial class frmEdit : Form
    {
        private TodoItem _item;

        public frmEdit(TodoItem item)
        {
            InitializeComponent();
            _item = item;
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            txtTodo.Text = _item.Name;
            cbbCompleted.SelectedIndex = _item.IsCompleted == true ? 0 : 1;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _item.Name = txtTodo.Text;
                _item.IsCompleted = cbbCompleted.SelectedIndex == 0 ? true : false;

                string json = JsonConvert.SerializeObject(_item, Formatting.None);
                string mensagem = new BoTodoItem().AtualizaTodoItem(_item);

                MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
