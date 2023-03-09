using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Presentation.Front.Enums;

namespace SnowaTec.Test.Presentation.Front.Interfaces
{
    public interface ICallApiService
    {
        Task<Response<T>> Call<T>(string token, string path, CallMethodType callMethodTypes, object? model = null, params (string name, object value)[] commandParameters);
    }
}
