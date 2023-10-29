using System.Net;

namespace DocumentsDBCaller
{
    public class DbResponse
    {

        public HttpStatusCode Status { get; set; }

        public object? Data { get; set; }

    }
}
