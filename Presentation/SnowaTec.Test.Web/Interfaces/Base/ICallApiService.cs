using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Web.Enums;

namespace SnowaTec.Test.Web.Interfaces.Base
{
    public interface ICallApiService
    {
        Task<Response<T>> Call<T>(string token, string path, CallMethodType callMethodTypes, object? model = null, params (string name, object value)[] commandParameters);
    }
}
