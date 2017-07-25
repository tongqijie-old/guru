using System;
using System.Threading.Tasks;
using Guru.AspNetCore.Abstractions;
using Guru.DependencyInjection;
using Guru.DependencyInjection.Attributes;
using Guru.ExtensionMethod;
using Guru.Formatter.Abstractions;

namespace Guru.AspNetCore.Implementations.Api
{
    [Injectable(typeof(IApiHandler), Lifetime.Singleton)]
    public class DefaultApiHandler : IApiHandler
    {
        private readonly IApiProvider _ApiProvider;

        private readonly IApiFormatter _ApiFormatter;

        public DefaultApiHandler(IApiProvider apiHandler, IApiFormatter apiFormater)
        {
            _ApiProvider = apiHandler;
            _ApiFormatter = apiFormater;
        }

        public async Task ProcessRequest(CallingContext context)
        {
            var apiContext = await _ApiProvider.GetApi(context);
            if (apiContext == null)
            {
                // TODO: log error
                return;
            }

            var result = apiContext.ApiExecute(apiContext.Parameters);
            if (result == null)
            {
                // TODO: log error
                return;
            }

            var contentType = "application/json";
            if (context.InputParameters.ContainsKey("accept"))
            {
                var accept = context.InputParameters["accept"].Value;
                if (accept.ContainsIgnoreCase("application/json"))
                {
                    contentType = "application/json";
                }
                else if (accept.ContainsIgnoreCase("application/xml"))
                {
                    contentType = "application/xml";
                }
                else if (accept.ContainsIgnoreCase("plain/text"))
                {
                    contentType = "plain/text";
                }
            }

            context.SetOutputParameter(new ContextParameter()
            {
                Name = "Content-Type",
                Source = ContextParameterSource.Header,
                Value = contentType,
            });
            
            if (contentType == "application/json")
            {
                await _ApiFormatter.GetFormatter("json").WriteObjectAsync(result, context.OutputStream);
            }
            else if (contentType == "application/xml")
            {
                await _ApiFormatter.GetFormatter("xml").WriteObjectAsync(result, context.OutputStream);
            }
            else
            {
                await _ApiFormatter.GetFormatter("text").WriteObjectAsync(result, context.OutputStream);
            }
        }
    }
}