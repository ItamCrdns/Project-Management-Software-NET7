namespace CompanyPMO_.NET.Dto
{
    public class ImageDto
    {
        public int ImageId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
