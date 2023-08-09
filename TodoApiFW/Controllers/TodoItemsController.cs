using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TodoLayers.BLL;

namespace TodoApiFW.Controllers
{
    [RoutePrefix("api/TodoItems")]
    public class TodoItemsController : ApiController
    {
        private BoTodoItem _boTodoItem = new BoTodoItem();

        /// <summary>
        /// Método usado para consultar todos os itens do banco de ToDoItem
        /// </summary>
        /// <returns>Lista de TodoItem</returns>
        [HttpPost]
        [Route("1")]
        public IHttpActionResult PostListaTodoItem()
        {
            try
            {
                JObject retornoJson = _boTodoItem.ListaTodoItem();
                return Ok(retornoJson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Usado para Incluir uma linha de registro de ToDoItem
        /// </summary>
        /// <returns>Retorna um Json string com mensagem de sucesso ou erro.</returns>
        [HttpPost]
        [Route("2")]
        public async Task<IHttpActionResult> PostIncluiTodoItem()
        {
            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                return Ok(_boTodoItem.Incluir(json));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Método usado para atualizar um item do banco através de Post.
        /// Ex: Url: localhost/api/TodoItems/up -> [Route("up")] e assinatura da classe -> [RoutePrefix("api/TodoItems")]
        /// </summary>
        /// <returns>Uma string dizendo que foi atualizada com sucesso</returns>
        [HttpPost]
        [Route("3")]
        public async Task<IHttpActionResult> PostUpdateTodoItem()
        {
            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                return Ok(_boTodoItem.Alterar(json));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Método usado para deletar um item do banco através de Post.
        /// </summary>
        /// <returns>Uma string dizendo que foi deletado com sucesso</returns>
        [HttpPost]
        [Route("4")]
        public async Task<IHttpActionResult> PostDeleteTodoItem()
        {
            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                return Ok(_boTodoItem.Deletar(json));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
