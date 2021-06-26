using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hangfire.Dashboard.Extensions
{
    internal sealed class StorageAssemblySingleton
    {
        private StorageAssemblySingleton()
        {
        }

        private static StorageAssemblySingleton _instance;
        private string[] prefixIgnore = new[] { "Hangfire.Dashboard.Extensions.dll", "Microsoft." };


        public List<Assembly> currentAssembly { get; private set; } = new List<Assembly>();

        internal static StorageAssemblySingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StorageAssemblySingleton();
            }
            return _instance;
        }

        internal void SetCurrentAssembly(bool includeReferences = true, params Assembly[] assemblies)
        {
            currentAssembly.AddRange(assemblies);

            if (includeReferences)
            {
                var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                var toLoad = referencedPaths.Where(r => !assemblies.Any(x => x.Location.Equals(r)))
                                .Where(x => !prefixIgnore.Any(p => p.Contains(x)))
                                .ToList();

                toLoad.ForEach(path => currentAssembly.Add(Assembly.LoadFile(path)));
            }

        }

        public bool IsValidType(string type) => currentAssembly.Any(x => x.GetType(type) != null);

        public Type GetClassType(string className)
        {
            return currentAssembly
                ?.FirstOrDefault(x => x.GetType(className) != null)
                ?.GetType(className);
        }

        public MethodInfo GetMethodInfo(string typeName, string methodName)
        {
            var type = GetClassType(typeName);
            if (type == null) return null;

            //如果是interface,则要拿到所有父接口(因为interface的GetMethod方法拿不到继承的父类的方法)
            if (type.IsInterface)
            {
                var interfaces = type.GetInterfaces().ToList();
                interfaces.Add(type);
                foreach (var t in interfaces)
                {
                    var mi = t.GetMethod(methodName);
                    if (mi != null) return mi;
                }
                return null;
            }
            else
            {
                return type?.GetMethod(methodName);
            }
        }
    }
}
