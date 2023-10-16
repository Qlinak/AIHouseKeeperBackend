using AIHouseKeeperBackend.DependencyInjections;

namespace AIHouseKeeperBackend.Configs;

public interface IAiConfig
{
   string Url { get; }
   
   string Key { get; }
}

public class AiConfig : IAiConfig, IConfig
{
   public string Url { get; }
   public string Key { get; }

   public AiConfig()
   {
      const EnvironmentVariableTarget target = EnvironmentVariableTarget.Process;
      Url = Environment.GetEnvironmentVariable("UST_URL", target) ??
                  throw new UnauthorizedAccessException("UST_URL NOT FOUND!");
      Key = Environment.GetEnvironmentVariable("UST_KEY", target) ??
            throw new UnauthorizedAccessException("UST_KEY NOT FOUND!");
   }
}