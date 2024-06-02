using RentIt.Domain.Abstractions;
using RentIt.Domain.Users.Events;

namespace RentIt.Domain.Users;

public sealed class User : Entity
{
    private readonly List<Role> m_Roles = [];

    private User(Guid id, FirstName firstName, LastName lastName, Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private User() { }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;

    public IReadOnlyCollection<Role> Roles => [.. m_Roles];

    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user.m_Roles.Add(Role.Registered);

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}