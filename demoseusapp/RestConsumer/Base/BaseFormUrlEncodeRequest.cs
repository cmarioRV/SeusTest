using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using demoseusapp.Services;

namespace demoseusapp
{
    public abstract class BaseFormUrlEncodeRequest<TResponse> : BaseRestConsumer<string, TResponse>
    {
        private string contentType;

        public BaseFormUrlEncodeRequest(HttpClientHandler handler, IStorage storage) : base(handler, storage)
        {
        }

        public TResponse ConsumeRestService(string body, string url, HttpMethod method, string contentType, Dictionary<string, string> headers = null, bool addAuthorization = true)
        {
            this.contentType = contentType;
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            return ConsumeRestService(body, request, headers, addAuthorization);
        }

        protected override void AddContent(string requestObject, HttpRequestMessage request)
        {
            if (request.Method == HttpMethod.Post)
            {
                request.Content = new StringContent(requestObject, Encoding.UTF8, new MediaTypeHeaderValue(contentType).ToString());
            }
        }
    }
}
