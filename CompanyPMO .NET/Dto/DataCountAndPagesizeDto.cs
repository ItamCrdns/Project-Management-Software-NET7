namespace CompanyPMO_.NET.Dto
{
    public class DataCountAndPagesizeDto<T>
    {
        public T Data { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
    }
}
