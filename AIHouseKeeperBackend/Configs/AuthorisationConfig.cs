using AIHouseKeeperBackend.DependencyInjections;

namespace AIHouseKeeperBackend.Configs;

public interface IAuthorisationConfig
{
    string SecretKey { get; }
}

public class AuthorisationConfig : IAuthorisationConfig, IConfig
{
    public string SecretKey { get; }

    public AuthorisationConfig()
    {
        const EnvironmentVariableTarget target = EnvironmentVariableTarget.Process;
        SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY", target) ??
                    throw new UnauthorizedAccessException("SECRET KEY NOT FOUND!");
    }
}