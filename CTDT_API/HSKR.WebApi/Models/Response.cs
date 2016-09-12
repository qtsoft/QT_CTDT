using System.Net.Http;

namespace CTDT.WebApi.Models
{
    public class Response<T> where T : class
    {
        public bool? Status { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public T Data { get; set; }

        public int? TotalRecord { get; set; }
       
    }
}