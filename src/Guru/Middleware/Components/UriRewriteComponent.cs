using System.Text.RegularExpressions;

using Guru.ExtensionMethod;
using Guru.DependencyInjection;
using Guru.Middleware.Abstractions;
using Guru.Middleware.Configuration;
using Guru.DependencyInjection.Attributes;

namespace Guru.Middleware.Components
{
    [Injectable(typeof(IUriRewriteComponent), Lifetime.Singleton)]
    internal class UriRewriteComponent : IUriRewriteComponent
    {
        public string Rewrite(string uri)
        {
            var rules = ContainerManager.Default.Resolve<IApplicationConfiguration>().Rewrites;
            if (rules.HasLength())
            {
                foreach (var rule in rules)
                {
                    if (Regex.IsMatch(uri, rule.Pattern, RegexOptions.IgnoreCase))
                    {
                        if (rule.Mode == RewriteMode.Override)
                        {
                            return rule.Value;
                        }
                        else if (rule.Mode == RewriteMode.Replace)
                        {
                            return Regex.Replace(uri, rule.Pattern, rule.Value);
                        }
                    }
                }
            }

            return uri;
        }
    }
}