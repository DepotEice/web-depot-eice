using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    public class ResultModel<T> where T : class
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode Code { get; set; }
        public string? Message { get; set; }
    }
}
