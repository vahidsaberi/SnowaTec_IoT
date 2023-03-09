using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Web.Contract;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Interfaces.Base;
using Remote.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using OrodGoverment.Web.Contract;

namespace SnowaTec.Test.Web.Services.Base
{
    public class CallApiService : ICallApiService
    {
        private readonly HttpClient _httpClient;

        public CallApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        public async Task<Response<T>> Call<T>(string token, string path, CallMethodType callMethodType, object? model = null, params (string name, object value)[] commandParameters)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                StringContent? data = null;
                HttpResponseMessage? response = null;

                foreach ((string Name, object Value) commandParameter in commandParameters)
                {
                    string name = commandParameter.Name;
                    object value = commandParameter.Value;

                    path = path.ToLower().Replace($"{{{name.ToLower()}}}", $"{value}");
                }

                while(path.Contains("?}"))
                {
                    var startIndex = path.IndexOf("{");
                    var endIndex = path.IndexOf("?}");

                    path = path.Remove((startIndex - 1), (endIndex - startIndex + 3));
                }

                switch (callMethodType)
                {
                    case CallMethodType.Get:
                        if (model != null)
                        {
                            var query = "?";
                            Type type = model.GetType();

                            foreach (PropertyInfo pi in type.GetProperties())
                            {
                                query += $"{pi.Name}={pi.GetValue(model, null)?.ToString()}&";
                            }

                            path += query;
                        }

                        response = await _httpClient.GetAsync(path);
                        break;
                    case CallMethodType.Post:
                        if (model is Expression)
                        {
                            var exp = model as Expression;
                            var query = LinqExpressioncs.SerialiseRemoteExpression(exp.ToRemoteLinqExpression());
                            data = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, StaticData.CONTENT_JSON);
                        }
                        else
                        {
                            data = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, StaticData.CONTENT_JSON);
                        }

                        response = await _httpClient.PostAsync(path, data);

                        break;
                    case CallMethodType.Put:
                        if (model is Expression)
                        {
                            var exp = model as Expression;
                            var query = LinqExpressioncs.SerialiseRemoteExpression(exp.ToRemoteLinqExpression());
                            data = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, StaticData.CONTENT_JSON);
                        }
                        else
                        {
                            data = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, StaticData.CONTENT_JSON);
                        }

                        response = await _httpClient.PutAsync(path, data);
                        break;
                    case CallMethodType.Delete:
                        response = await _httpClient.DeleteAsync(path);
                        break;
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStreamAsync();

                    var item = await JsonSerializer.DeserializeAsync<Response<T>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return item;
                }
                else
                {
                    return new Response<T> { 
                        Message = "خطا در عملیات.", 
                        Errors = new List<string> 
                        { 
                            path,
                            response.ToString(),
                            model.ToString(),
                            Newtonsoft.Json.JsonConvert.SerializeObject(model)
                        } 
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<T> { Succeeded = false, Message = ex.Message };
            }
        }
    }
}
