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
        public DateTime Created { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime LastLogin { get; set; }
        public int CompanyId { get; set; }
        public int TierId { get; set; }
        public int? SupervisorId { get; set; }
    }

    public class EmployeePasswordDto
    {
        public string Password { get; set; }
    }
}
