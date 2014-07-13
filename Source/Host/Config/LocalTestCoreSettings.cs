using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Core.Configuration;

namespace Thinktecture.IdentityServer.Host.Config
{
    public class LocalTestCoreSettings : CoreSettings
    {
        private readonly string _issuerUri;
        private readonly string _siteName;
        private readonly X509Certificate2 _certificate;
        private readonly string _publicHostAddress;

        public LocalTestCoreSettings(string issuerUri, string siteName, string publicHostAddress)
        {
            _issuerUri = issuerUri;
            _siteName = siteName;
            _publicHostAddress = publicHostAddress;

            var assembly = GetType().Assembly;
            using (var stream = assembly.GetManifestResourceStream("Thinktecture.IdentityServer.Host.Config.idsrv3test.pfx"))
            {
                _certificate = new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        public override X509Certificate2 SigningCertificate
        {
            get
            {
                if (_certificate == null)
                {
                    throw new InvalidOperationException("No certificate specified.");
                }

                return _certificate;
            }
        }

        public override string IssuerUri
        {
            get { return _issuerUri; }
        }

        public override string SiteName
        {
            get { return _siteName; }
        }

        public override string PublicHostName
        {
            get { return _publicHostAddress; }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}