using RentIt.Application.Abstractions.Messaging;

namespace RentIt.Application.Users.GetLoggedInUser;
public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;