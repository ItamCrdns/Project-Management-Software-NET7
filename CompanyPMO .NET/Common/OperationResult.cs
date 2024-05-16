namespace CompanyPMO_.NET.Common
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
