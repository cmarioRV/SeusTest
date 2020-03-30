using System.Net.Http;

namespace demoseusapp
{
    public interface IManagerHttpMessageResponse<T>
    {
        T ManageResponseMessages(HttpResponseMessage response);
    }
}
