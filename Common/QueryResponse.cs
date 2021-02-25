using System;
using System.Collections.Generic;

namespace Common
{
    [Serializable]
    public class QueryResponse<T> : Response
    {
        public List<T> Data { get; set; }
    }
    [Serializable]
    public class SingleResponse<T> : Response
    {
        public T Data { get; set; }
        
    }
}
