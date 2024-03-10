using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IResetPasswordRequest
    {
        int GenerateResetPasswordToken();
        Task<OperationResult<Guid>> RequestPasswordReset(string email);
        Task<OperationResult<bool>> ResetPasswordWithToken(string email, int token, string newPassword);
        public Task<OperationResult<string>> RequestExists(Guid requestGuid);
        Task<OperationResult<bool>> IsTokenValid(int token, Guid requestGuid);
    }
}
