using Microsoft.Extensions.Configuration;

namespace Ray.Infrastructure.Config
{
    /// <summary>
    /// 自定义的排除空值的环境变量配置源
    /// （用于取待默认的<see cref="EnvironmentVariablesConfigurationSource"/>）
    /// </summary>
    public class EnvironmentVariablesExcludeEmptyConfigurationSource : IConfigurationSource
    {
        public string Prefix { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariablesExcludeEmptyConfigurationProvider(Prefix);
        }
    }
}
