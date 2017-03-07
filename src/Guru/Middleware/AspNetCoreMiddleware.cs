﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Guru.ExtensionMethod;
using Guru.DependencyInjection;
using Guru.Middleware.Abstractions;

namespace Guru.Middleware
{
    public class AspNetCoreMiddleware
    {
        private readonly RequestDelegate _Next;

        private readonly IMiddlewareLifetime _Lifetime;

        private readonly IUriRewriteComponent _UriRewriteComponent;

        private readonly IDefaultUriComponent _DefaultUriComponent;

        private readonly IHttpHandlerComponent _HttpHandlerComponent;

        public AspNetCoreMiddleware(RequestDelegate next, IMiddlewareLifetime lifetime)
        {
            _Next = next;
            _Lifetime = lifetime;

            var loader = new DefaultAssemblyLoader();
            ContainerEntry.Init(loader);

            _UriRewriteComponent = ContainerEntry.Resolve<IUriRewriteComponent>();
            _DefaultUriComponent = ContainerEntry.Resolve<IDefaultUriComponent>();
            _HttpHandlerComponent = ContainerEntry.Resolve<IHttpHandlerComponent>();

            if (_Lifetime != null)
            {
                _Lifetime.Startup(ContainerEntry.Container);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value.Trim('/');

            path = _UriRewriteComponent.Rewrite(path);

            if (!path.HasValue())
            {
                path = _DefaultUriComponent.Default();
            }

            await _HttpHandlerComponent.Process(path, context);
        }
    }
}