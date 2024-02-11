namespace CompanyPMO_.NET.Dto
{
    public class DataCountAndPagesizeDto<T>
    {
        // T should always be a list. Too lazy to change it now
        public T Data { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
    }
}
