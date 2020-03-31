using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using demoseusapp.Repositories;
using demoseusapp.Services;

namespace demoseusapp
{
    public abstract class BaseRestConsumer<TRequest, TResponse>
    {
        private readonly ISeusRepository seusRepository;
        private readonly IStorage storage;

        private const string authorizationHeader = "X-APPTAG-TOKEN";

        private readonly double timeOutInSeconds = 60;
        private readonly HttpClient client;

        private HttpResponseMessage response;

        protected BaseRestConsumer(NSUrlSessionHandler handler, IStorage storage)
        {
            this.storage = storage;

            client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(timeOutInSeconds)
            };

            seusRepository = ServiceLocator.Instance.Get<ISeusRepository>();
        }

        public TResponse ConsumeRestService(TRequest requestObject, HttpRequestMessage request, Dictionary<string, string> headers = null, bool addAuthorization = true)
        {
            Debug.WriteLine("URL: " + request.RequestUri);
            AddHeaders(headers, addAuthorization);
            AddContent(requestObject, request);
            response = client.SendAsync(request).Result;
            return ManageResponseMessages(response);

            throw new Exception("Error consumiendo el servicio");
        }

        protected abstract TResponse ManageResponseMessages(HttpResponseMessage response);
        protected abstract void AddContent(TRequest requestObject, HttpRequestMessage request);

        private void AddHeaders(Dictionary<string, string> headers, bool addAuthorization = true)
        {
            if (addAuthorization)
            {
                string accessToken = storage.GetAccessToken();
                //client.DefaultRequestHeaders.Add(authorizationHeader, accessToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
    }
}
