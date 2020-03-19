using System.Net.Http;

namespace Angus.Bills.HTTP.HttpClient
{
    public class HttpResult<T>
    {
        public HttpResult(T result, HttpResponseMessage response)
        {
            Result = result;
            Response = response;
        }

        public T Result { get; }
        public HttpResponseMessage Response { get; }
        public bool HasResult => Result is {};
    }
}