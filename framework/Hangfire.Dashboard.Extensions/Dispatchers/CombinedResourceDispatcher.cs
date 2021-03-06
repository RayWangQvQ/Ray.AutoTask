﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Annotations;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    /// <summary>
    /// [调度器]混合多个Resource
    /// </summary>
    public class CombinedResourceDispatcher : EmbeddedResourceDispatcher
    {
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly string[] _resourceNames;

        public CombinedResourceDispatcher(
            [NotNull] string contentType,
            [NotNull] Assembly assembly,
            string baseNamespace,
            params string[] resourceNames) : base(contentType, assembly, null)
        {
            _assembly = assembly;
            _baseNamespace = baseNamespace;
            _resourceNames = resourceNames;
        }

        protected override async Task WriteResponse(DashboardResponse response)
        {
            foreach (var resourceName in _resourceNames)
            {
                await WriteResource(
                    response,
                    _assembly,
                    $"{_baseNamespace}.{resourceName}").ConfigureAwait(false);
            }
        }
    }
}
