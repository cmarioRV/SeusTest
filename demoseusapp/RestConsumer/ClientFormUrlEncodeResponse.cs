using System.Net.Http;
using demoseusapp.Services;

namespace demoseusapp
{
    public class ClientFormUrlEncodeResponse : BaseFormUrlEncodeRequest<string>
    {
        private readonly IManagerHttpMessageResponse<string> managerMessageResponse;

        public ClientFormUrlEncodeResponse(HttpClientHandler handler, IStorage storage) : base(handler, storage)
        {
            managerMessageResponse = new ManagerHttpFormUrlEncode();
        }

        protected override string ManageResponseMessages(HttpResponseMessage response)
        {
            return managerMessageResponse.ManageResponseMessages(response);
        }
    }
}
