using RentIt.Domain.Users;

namespace RentIt.Application.Abstractions.Email;
public interface IEmailService
{
    Task SendAsync(Domain.Users.Email recipient, string subject, string body);
}
