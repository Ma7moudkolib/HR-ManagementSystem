namespace Shared.DataTransferObjects
{
    public record ServiceResponse(bool Success = false, string message = null!);
}
