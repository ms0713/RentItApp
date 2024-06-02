using RentIt.Application.Abstractions.Authentication;
using RentIt.Application.Abstractions.Messaging;
using RentIt.Domain.Abstractions;
using RentIt.Domain.Users;

namespace RentIt.Application.Users.LogInUser;
internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    private readonly IJwtService m_JwtService;

    public LogInUserCommandHandler(IJwtService jwtService)
    {
        m_JwtService = jwtService;
    }

    public async Task<Result<AccessTokenResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await m_JwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        return new AccessTokenResponse(result.Value);
    }
}
