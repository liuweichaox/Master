using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Virgo.AspNetCore
{
    /// <summary>
    /// 获取配置辅助类
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 获取<see cref="IConfigurationRoot"/>实例
        /// </summary>
        /// <param name="basthPath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string basthPath)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(!string.IsNullOrEmpty(basthPath) ? basthPath : Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        /// <summary>
        /// 获取指定环境的<see cref="IConfigurationRoot"/>实例
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="basthPath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string environment, string basthPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(!string.IsNullOrEmpty(basthPath) ? basthPath : Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// 获取指定节点实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public static T GetApplicationConfiguration<T>(IConfigurationRoot iConfig)
        {
            var configuration = default(T);

            iConfig?.GetSection(typeof(T).Name)?.Bind(configuration);

            return configuration;
        }

        /// <summary>
        /// 获取指定节点实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iConfig"></param>
        /// <returns></returns>
        public static IConfigurationSection GetConfigurationSection<T>(IConfigurationRoot iConfig)
        {
            return iConfig?.GetSection(typeof(T).Name);
        }
    }
}
