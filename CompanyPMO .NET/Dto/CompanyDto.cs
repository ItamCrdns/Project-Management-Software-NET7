namespace CompanyPMO_.NET.Dto
{
    public class CompanyDto
    {
        public string? Name { get; set; }
        public int CeoUserId { get; set; }
        public int AddressId { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public PatchEntityImagesDto? Images { get; set; }
    }
}
