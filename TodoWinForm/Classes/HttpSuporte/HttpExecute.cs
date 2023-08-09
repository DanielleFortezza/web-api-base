using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TodoWinForm.Classes.HttpSuporte
{
    public class HttpExecute
    {
        /// <summary>
        /// Busca uma lista genérica.
        /// </summary>
        /// <typeparam name="T">Qualquer tipo desejado.</typeparam>
        /// <param name="jsonParametros">Todos os parâmetros necessários em uma única string. Separador = ";"</param>
        /// <param name="uri">URL de destino da WEBAPI.</param>
        /// <param name="token">Token de sessão para validação.</param>
        /// <returns>Retorna uma lista do tipo informado.</returns>
        public static List<T> HTTPBuscarLista<T>(string jsonParametros, string uri, string token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpContent content;

                    if (jsonParametros != null)
                    {
                        content = new StringContent(jsonParametros, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        content = null;
                    }

                    HttpResponseMessage response = client.PostAsync(new Uri(uri), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        List<T> Lista = JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
                        return Lista;
                    }
                    else
                    {
                        JObject jErro = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        string erro = jErro["Message"].ToString();
                        throw new Exception(erro);
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Busca um resultado específico do tipo desejado.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno desejado.</typeparam>
        /// <param name="jsonParametros">Todos os parâmetros necessários em uma única string. Separador = ";"</param>
        /// <param name="uri">URL de destino da WEBAPI.</param>
        /// <param name="token">Token de sessão para validação.</param>
        /// <returns>Retorna um objeto do tipo informado.</returns>
        public static T HTTPBuscarResultado<T>(string jsonParametros, string uri, string token)
        {
            try
            {
                HttpContent content;

                if (jsonParametros != null)
                {
                    content = new StringContent(jsonParametros, Encoding.UTF8, "application/json");
                }
                else
                {
                    content = null;
                }

                using (HttpClient client = new HttpClient())
                {

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = client.PostAsync(new Uri(uri), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var resultado = response.Content.ReadAsStringAsync().Result;
                        T obj;

                        try
                        {
                           obj = JsonConvert.DeserializeObject<T>(resultado);
                        }
                        catch (Exception)
                        {
                            obj = (T)Convert.ChangeType(resultado, typeof(T));
                        }
                        
                        return obj;
                    }
                    else
                    {
                        if (response.Content.ReadAsStringAsync().Result.Contains("404"))
                        {
                            throw new Exception("O caminho URL está inválido. (404 - Not Found).");
                        }
                        //JObject jErro = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        string erro = response.Content.ReadAsStringAsync().Result;//jErro["Message"].ToString();
                        throw new Exception(erro);
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);
                throw new AggregateException("Não foi possível obter uma resposta da WebAPI. Verifique a conexão.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Execute um comando na WebApi.
        /// </summary>
        /// <param name="jsonParametros"></param>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        public static void HTTPExecuteVoid(string jsonParametros, string uri, string token)
        {
            try
            {
                HttpContent content = new StringContent(jsonParametros, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = client.PostAsync(new Uri(uri), content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        JObject jErro = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        string erro = jErro["Message"].ToString();
                        throw new Exception(erro);
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        public static DataTable HTTPDataTable(HttpContent content, string Uri, string ChaveAcesso)
        {
            DataTable Lista = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    // Json de Response //
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    var response = client.PostAsync(new Uri(Uri), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Lista = (DataTable)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, (typeof(DataTable)));
                        foreach (DataRow l in Lista.Rows)
                        {
                            for (int i = 0; i <= Lista.Columns.Count - 1; i++)
                            {
                                if (l[i].ToString().Contains("Sessão expirada"))
                                {
                                    MessageBox.Show(l[i].ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                                    Form.Show();
                                }
                            }

                        }
                    }
                    else
                    {
                        var erro = JsonConvert.DeserializeObject<Error_>(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show(erro.error_description, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (erro.error_description.Contains("Sessão expirada"))
                        {
                            Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                            Form.Show();
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Lista;

        }

        public static DataSet HTTPDataSet(HttpContent content, string Uri, string ChaveAcesso)
        {
            DataSet Tables = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    // Json de Response //
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    var response = client.PostAsync(new Uri(Uri), content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Tables = (DataSet)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, (typeof(DataSet)));
                        foreach (DataRow l in Tables.Tables[0].Rows)
                        {
                            for (int i = 0; i <= Tables.Tables[0].Columns.Count - 1; i++)
                            {
                                if (l[i].ToString().Contains("Sessão expirada"))
                                {
                                    MessageBox.Show(l[i].ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                                    Form.Show();
                                    //Form.Visible=true;

                                }
                            }

                        }
                    }
                    else
                    {
                        var erro = JsonConvert.DeserializeObject<Error_>(response.Content.ReadAsStringAsync().Result);
                        MessageBox.Show(erro.error_description, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (erro.error_description.Contains("Sessão expirada"))
                        {
                            Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                            Form.Show();
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Tables;

        }

        public static string HTTPString(HttpContent content, string Uri, string ChaveAcesso)
        {
            string Resultado = null;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChaveAcesso);

                    // Json de Response //
                    var response = client.PostAsync(new Uri(Uri), content).Result;

                    Resultado = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);

                    if (response.IsSuccessStatusCode)
                    {
                        if (Resultado != null)
                        {
                            if (Resultado.Contains("Sessão expirada"))
                            {
                                MessageBox.Show(Resultado.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                                Form.Show();
                            }
                            else
                            {
                                if (Resultado.Contains("Erro"))
                                {
                                    MessageBox.Show(Resultado.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return Resultado;
                                }
                                else
                                {
                                    return Resultado;
                                }
                            }
                        }
                        else
                        {
                            return Resultado;
                        }

                    }
                    else
                    {
                        MessageBox.Show(Resultado.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (Resultado.Contains("Sessão expirada"))
                        {
                            Form = new FormAutenticacao(TGSProdCardsModulo1.UsuarioAutenticado.Login);
                            Form.Show();
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Ocorreu um erro inesperado na chamada http: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Resultado;

        }
        */
    }
}
