using System.Collections.Generic;

namespace API.Domain
{
    public class DefaultResult
    {
        public bool Success { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }

    public class DefaultResult<T> : DefaultResult
    {
        public DefaultResult() { }

        public DefaultResult(T data)
        {
            Data = data;
            Success = true;
        }

        public T Data;
    }
}
