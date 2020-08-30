namespace Imposto.Core.Service
{
    public class Result
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
