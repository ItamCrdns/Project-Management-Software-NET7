namespace CompanyPMO_.NET.Dto
{
    public class EmployeeRegisterDto
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset Created { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public int CompanyId { get; set; }
        public int TierId { get; set; }
    }
}
