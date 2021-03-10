using System;
using System.Collections.Generic;
using System.Net;

namespace TokenAuthentication.Dtos
{
    public class ResponseDto<T> where T : class 
    {   
        public T Data { get; set; }
        public Nullable<Guid> Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }        
    }
}
