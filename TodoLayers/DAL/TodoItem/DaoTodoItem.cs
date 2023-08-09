using System.Collections.Generic;
using System.Data;
using TodoLayers.DML;
using System.Data.SqlClient;

namespace TodoLayers.DAL
{
    internal class DaoTodoItem : AcessoDados
    {
        internal List<TodoItem> Pesquisa()
        {

            DataSet ds = base.Consultar("SP_PesTodoItem", null);
            List<TodoItem> tdi = Converter(ds);

            return tdi;
        }

        internal long Incluir(TodoItem item)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Name", item.Name),
                new SqlParameter("IsCompleted", item.IsCompleted)
            };

            DataSet ds = base.Consultar("SP_IncTodoItem", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal string Alterar(TodoItem item)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Name", item.Name),
                new SqlParameter("IsCompleted", item.IsCompleted),
                new SqlParameter("Id", item.Id)
            };

            DataSet ds = base.Consultar("SP_AltTodoItem", parametros);
            string ret = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
                ret = ds.Tables[0].Rows[0][0].ToString();
            return ret;
        }

        internal string Deletar(TodoItem item)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", item.Id)
            };

            DataSet ds = base.Consultar("SP_DelTodoItem", parametros);
            string ret = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
                ret = ds.Tables[0].Rows[0][0].ToString();
            return ret;
        }

        private List<TodoItem> Converter(DataSet ds)
        {
            List<TodoItem> lista = new List<TodoItem>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TodoItem tdi = new TodoItem
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name"),
                        IsCompleted = row.Field<bool>("IsCompleted")
                    };
                    lista.Add(tdi);
                }
            }
            else
            {
                lista = null;
            }

            return lista;
        }
    }
}
