namespace CompanyPMO_.NET.Dto
{
    public class LoginResponseDto
    {
        public AuthenticationResult Result { get; set; }
        public string Message { get; set; }
        public EmployeeDto Employee { get; set; }
    }

    public class AuthenticationResult
    {
        public bool WrongCreds { get; set; }
        public bool Blocked { get; set; }
        public bool SomethingWrong { get; set; }
        public bool Authenticated { get; set; }
        public bool DoesntExist { get; set; }
    }
}
