using System.Diagnostics;
using System.Net.Http;

namespace demoseusapp
{
    public class ManagerHttpFormUrlEncode : IManagerHttpMessageResponse<string>
    {
        public string ManageResponseMessages(HttpResponseMessage response)
        {
            string responseString = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Response FormUrlEncode: " + responseString);
            return responseString;
        }
    }
}
