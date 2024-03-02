namespace CompanyPMO_.NET.Common
{
    public class DataCountPages<T>
    {
        public IEnumerable<T>? Data { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
    }
}