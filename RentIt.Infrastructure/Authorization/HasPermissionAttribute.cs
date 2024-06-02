using Microsoft.AspNetCore.Authorization;

namespace RentIt.Infrastructure.Authorization;
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
