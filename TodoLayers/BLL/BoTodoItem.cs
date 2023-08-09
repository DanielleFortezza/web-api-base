using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TodoLayers.DAL;
using TodoLayers.DML;

namespace TodoLayers.BLL
{
    /// <summary>
    /// Classe de regras de negócio entre o acesso aos dados e a chamada do controlador.
    /// Trata o retorno do banco, converte para Json e monta frases de retorno.
    /// </summary>
    public class BoTodoItem
    {
        /// <summary>
        /// Método que busca as tarefas do banco e formata o retorno de acordo.
        /// </summary>
        /// <returns>Uma lista de tarefas.</returns>
        public JObject ListaTodoItem()
        {
            DaoTodoItem tdi = new DaoTodoItem();

            List<TodoItem> itens = tdi.Pesquisa();
            string jsonRetorno;

            if (itens == null)
                jsonRetorno = JsonConvert.SerializeObject(new { Retorno = "Não há dados armazenados." }, Formatting.None);
            else
                jsonRetorno = JsonConvert.SerializeObject(new { TodoItems = itens }, Formatting.None);
            
            return JObject.Parse(jsonRetorno);
        }

        /// <summary>
        /// Método que trata as informações de uma tarefa para inclusão no banco de dados.
        /// </summary>
        /// <param name="jsonItem">Item tarefa formatado em Json.</param>
        /// <returns>Id inserido no banco.</returns>
        public long Incluir(string jsonItem)
        {
            DaoTodoItem tdi = new DaoTodoItem();
            TodoItem item = JsonConvert.DeserializeObject<TodoItem>(jsonItem);
            return tdi.Incluir(item);
        }

        /// <summary>
        /// Método que trata as informações de uma tarefa para alteração no banco de dados.
        /// </summary>
        /// <param name="jsonItem">Item tarefa formatado em Json.</param>
        /// <returns>Uma frase informativa.</returns>
        public JObject Alterar(string jsonItem)
        {
            DaoTodoItem tdi = new DaoTodoItem();
            TodoItem item = JsonConvert.DeserializeObject<TodoItem>(jsonItem);
            string retorno = tdi.Alterar(item);
            return JObject.Parse(JsonConvert.SerializeObject(new { Retorno = retorno }, Formatting.None));
        }

        /// <summary>
        /// Método que trata as informações de uma tarefa para remoção no banco de dados.
        /// </summary>
        /// <param name="jsonItem">Item tarefa formatado em Json.</param>
        /// <returns>Uma frase informativa.</returns>
        public JObject Deletar(string jsonItem)
        {
            DaoTodoItem tdi = new DaoTodoItem();
            TodoItem item = JsonConvert.DeserializeObject<TodoItem>(jsonItem);
            string retorno = tdi.Deletar(item);
            return JObject.Parse(JsonConvert.SerializeObject(new { Retorno = retorno }, Formatting.None));
        }
    }
}
