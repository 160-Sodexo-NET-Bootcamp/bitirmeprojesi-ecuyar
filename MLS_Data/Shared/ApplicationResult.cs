using System;

namespace MLS_Data.Shared
{
    public class ApplicationResult<T> : CommonApplicationResult
    {
        public T Result { get; set; }
    }

    public class ApplicationResult : CommonApplicationResult
    {
    }

    public class CommonApplicationResult
    {
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
}
