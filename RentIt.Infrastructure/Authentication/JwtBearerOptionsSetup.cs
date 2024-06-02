using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace RentIt.Infrastructure.Authentication;
internal sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions m_AuthenticationOptions;

    public JwtBearerOptionsSetup(IOptions<AuthenticationOptions> authenticationOptions)
    {
        m_AuthenticationOptions = authenticationOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.Audience = m_AuthenticationOptions.Audience;
        options.MetadataAddress = m_AuthenticationOptions.MetadataUrl;
        options.RequireHttpsMetadata = m_AuthenticationOptions.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidIssuer = m_AuthenticationOptions.Issuer;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
