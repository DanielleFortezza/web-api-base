using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TodoLayers.DAL
{
    internal class AcessoDados
    {
        private string StringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal void Executar(string NomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(StringDeConexao))
            {
                using (SqlCommand comando = new SqlCommand(NomeProcedure, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 400;
                    if (parametros != null)
                        foreach (var item in parametros)
                            comando.Parameters.Add(item);

                    try
                    {
                        conexao.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
        }

        internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        {
            using (SqlConnection conexao = new SqlConnection(StringDeConexao))
            {
                using (SqlCommand comando = new SqlCommand(NomeProcedure, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 400;
                    if (parametros != null)
                        foreach (var item in parametros)
                            comando.Parameters.Add(item);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            try
                            {
                                conexao.Open();
                                adapter.Fill(ds);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                conexao.Close();
                            }

                            return ds;
                        }
                    }
                }
            }
        }

    }
}
