using RentIt.Application.Abstractions.Messaging;

namespace RentIt.Application.Users.LogInUser;
public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;