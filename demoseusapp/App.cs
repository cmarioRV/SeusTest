using System;
using demoseusapp.Repositories;
using demoseusapp.Repositories.Seus;

namespace demoseusapp
{
    public class App
    {
        public static bool UseMockDataStore = false;
        public static string BackendUrl = "http://localhost:5000";

        public static void Initialize()
        {
            if (UseMockDataStore)
            {
                ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
                ServiceLocator.Instance.Register<ISeusRepository, MockSeusRepository>();
            }
            else
            {
                ServiceLocator.Instance.Register<IDataStore<Item>, CloudDataStore>();
                ServiceLocator.Instance.Register<ISeusRepository, SeusRepository>();
            }
        }
    }
}
