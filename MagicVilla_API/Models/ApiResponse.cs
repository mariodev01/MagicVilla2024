using System.Net;
using System.Runtime.InteropServices.ObjectiveC;

namespace MagicVilla_API.Models
{
    public class ApiResponse
    {

        public HttpStatusCode statusCode { get; set; }

        public bool IsExitoso { get; set; } = true;

        public List<string> ErrorMessage { get; set; }

        public object Resultado { get; set; }
    }
}
