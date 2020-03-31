using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using demoseusapp.Services;
using Newtonsoft.Json;

namespace demoseusapp
{
    public class RestConsumerJson<TRequest, TResponse> : BaseRestConsumer<TRequest, TResponse>
    {
        private const string mediaType = "application/json";
        private IManagerHttpMessageResponse<TResponse> managerMessageResponse;

        public RestConsumerJson(NSUrlSessionHandler handler, IStorage storage) : base(handler, storage)
        {
            managerMessageResponse = new ManagerHttpJsonResponse<TResponse>();
        }

        public TResponse ConsumeRestService(TRequest requestObject, string url, HttpMethod method, Dictionary<string, string> headers = null, double timeOutInSeconds = 60)
        {
            var request = new HttpRequestMessage(method, url);
            return ConsumeRestService(requestObject, request, headers);
        }

        protected override void AddContent(TRequest requestObject, HttpRequestMessage request)
        {
            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                string serializedRequestObject = JsonConvert.SerializeObject(requestObject);
                Debug.WriteLine("Body: " + serializedRequestObject);
                StringContent serializedRequest = new StringContent(serializedRequestObject, Encoding.UTF8, mediaType);
                request.Content = serializedRequest;
            }
        }

        protected override TResponse ManageResponseMessages(HttpResponseMessage response)
        {
            return managerMessageResponse.ManageResponseMessages(response);
        }
    }
}
