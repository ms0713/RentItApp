using RentIt.Application.Abstractions.Messaging;

namespace RentIt.Application.Users.RegisterUser;
public sealed record RegisterUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;
