using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace demoseusapp
{
    public class HTTPErrorHandler
    {/*
        private ErrorResponse errorResponse;
        private readonly HttpStatusCode statusCode;

        public HTTPErrorHandler(string responseString, HttpStatusCode statusCode)
        {
            ParseResponse(responseString);
            this.statusCode = statusCode;
        }

        private void ParseResponse(string responseString)
        {
            try
            {
                errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                throw new UnexpectedException();
            }

        }

        public Exception HandleRequest()
        {
            Exception exception;
            switch ((int)statusCode)
            {
                case 500:
                    exception = new InternalServerException();
                    break;
                case 400:
                case 403:
                case 404:
                case 409:
                case 498:
                    exception = new SegurosSuraException(errorResponse.Error.Errors[0].Reason);
                    break;
                case 504:
                    exception = new TimeoutException(HTTPStrings.TimeoutExceeded);
                    break;
                default:
                    exception = new UnexpectedException();
                    break;
            }
            return exception;
        }*/
    }
}