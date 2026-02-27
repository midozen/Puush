namespace Puush.Contracts.Api.Responses.Enums;

public enum ResponseCode
{
    Success = 0,
    AuthenticationFailure = -1,
    Unknown = -2,
    ChecksumError = -3,
    InsufficientStorage = -4,
}
