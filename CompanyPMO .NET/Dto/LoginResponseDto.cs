namespace CompanyPMO_.NET.Dto
{
    public class LoginResponseDto
    {
        public bool Authenticated { get; set; }
        public string Message { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
