using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;

namespace demoseusapp
{
    public class ManagerHttpJsonResponse<T> : IManagerHttpMessageResponse<T>
    {
        public T ManageResponseMessages(HttpResponseMessage response)
        {
            string responseString = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Json Response: " + responseString);

            if (response.IsSuccessStatusCode)
            {
                T serializedObject = JsonConvert.DeserializeObject<T>(responseString);
                return serializedObject;
            }

            throw new System.Exception();
        }
    }
}
