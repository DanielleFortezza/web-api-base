using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TodoWinForm.Classes.HttpSuporte;

namespace TodoWinForm.Classes
{
    class BoTodoItem
    {

        public enum Rotas
        {
            Pesquisar = 1,
            Incluir = 2,
            Alterar = 3,
            Deletar = 4
        };

        /// <summary>
        /// Rota para acesso ao controller API UsuariosController
        /// </summary>
        private string RotaTodoItemAPI { get; } = @"/api/TodoItems/";

        public List<TodoItem> BuscarTodoItems()
        {
            //Montando uma string com o endereço do servidor (API) e o caminho URL que deverá "executar"
            string Uri = Program.API + RotaTodoItemAPI + (int)BoTodoItem.Rotas.Pesquisar;

            try
            {
                List<TodoItem> items = new List<TodoItem>();
                string retorno = HttpExecute.HTTPBuscarResultado<string>(null, Uri, null);
                if (JObject.Parse(retorno)["Retorno"] != null)
                {
                    retorno = JObject.Parse(retorno)["Retorno"].ToString();
                    throw new Exception(retorno);
                }
                else
                {
                    items = JsonConvert.DeserializeObject<List<TodoItem>>(JObject.Parse(retorno)["TodoItems"].ToString());
                }
                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsereTodoItem(TodoItem item)
        {
            string Uri = Program.API + RotaTodoItemAPI + (int)Rotas.Incluir;
            string parametros = JsonConvert.SerializeObject(item, Formatting.None);

            try
            {
                string retorno = HttpExecute.HTTPBuscarResultado<string>(parametros, Uri, null);
                return int.Parse(retorno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AtualizaTodoItem(TodoItem item)
        {
            string Uri = Program.API + RotaTodoItemAPI + (int)Rotas.Alterar;
            string parametros = JsonConvert.SerializeObject(item, Formatting.None);

            try
            {
                string retorno = HttpExecute.HTTPBuscarResultado<string>(parametros, Uri, null);
                return JObject.Parse(retorno)["Retorno"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ExcluiTodoItem(TodoItem item)
        {
            string Uri = Program.API + RotaTodoItemAPI + (int)Rotas.Deletar;
            string parametros = JsonConvert.SerializeObject(item, Formatting.None);

            try
            {
                string retorno = HttpExecute.HTTPBuscarResultado<string>(parametros, Uri, null);
                return JObject.Parse(retorno)["Retorno"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
