using Owin;
using SelfHost.Config;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Logging;

namespace SelfHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            var options = new IdentityServerOptions
            {
                SiteName = "Thinktecture IdentityServer3 (EntityFramework)",
                SigningCertificate = Certificate.Get(),
                Factory = Factory.Configure("IdSvr3Config")
            };

            appBuilder.UseIdentityServer(options);
        }
    }
}