using RentIt.Domain.Users;

namespace RentIt.Application.Abstractions.Authentication;
public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default);
}
