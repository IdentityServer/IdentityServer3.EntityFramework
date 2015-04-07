using Owin;
using SelfHost.Config;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Logging;

namespace SelfHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            var options = new IdentityServerOptions
            {
                SiteName = "IdentityServer3 (EntityFramework)",
                SigningCertificate = Certificate.Get(),
                Factory = Factory.Configure("IdSvr3Config")
            };

            appBuilder.UseIdentityServer(options);
        }
    }
}