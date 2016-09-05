using Owin;
using WebHost.Config;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Logging;
using Serilog;

namespace WebHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(@"c:\logs\ef.txt")
               .CreateLogger();

            appBuilder.Map("/core", core =>
            {
                var options = new IdentityServerOptions
                {
                    SiteName = "IdentityServer3 (EntityFramework)",
                    SigningCertificate = Certificate.Get(),
                    Factory = Factory.Configure("IdSvr3Config")
                };

                core.UseIdentityServer(options);
            });
        }
    }
}