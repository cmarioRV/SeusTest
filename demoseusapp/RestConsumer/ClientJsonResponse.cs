using System.Net.Http;
using demoseusapp.Services;

namespace demoseusapp
{
    public class ClientJsonResponse<T> : BaseFormUrlEncodeRequest<T>
    {
        private IManagerHttpMessageResponse<T> managerMessageResponse;

        public ClientJsonResponse(NSUrlSessionHandler handler, IStorage storage) : base(handler, storage)
        {
            managerMessageResponse = new ManagerHttpJsonResponse<T>();
        }

        protected override T ManageResponseMessages(HttpResponseMessage response)
        {
            return managerMessageResponse.ManageResponseMessages(response);
        }
    }
}
