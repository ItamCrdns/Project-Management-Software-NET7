using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IResetPasswordRequest
    {
        int GenerateResetPasswordToken();
        Task<OperationResult<Guid>> RequestPasswordReset(string email);
        Task<OperationResult<string>> ResetPasswordWithToken(string email, int token, string newPassword);
        Task<OperationResult<bool>> ResetPasswordWithCurrentPassword(int employeeId, string currentPassword, string newPassword);
        public Task<OperationResult<string>> RequestExists(Guid requestGuid);
        Task<OperationResult<bool>> IsTokenValid(int token, Guid requestGuid);
    }
}
