using System;

namespace demoseusapp.Models
{
    public class ExpiredTokenException: Exception
    {
        public ExpiredTokenException(string message): base(message)
        {
        }
    }
}