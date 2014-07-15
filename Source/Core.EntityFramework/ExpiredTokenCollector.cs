using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Thinktecture.IdentityServer.Core.EntityFramework
{
    public class ExpiredTokenCollector
    {
        private static string _connectionString;
        private static readonly Timer Timer = new Timer();

        public static void Start(string connectionString, int cleanupIntervalInMinutes)
        {
            _connectionString = connectionString;

            Timer.AutoReset = true;
            Timer.Interval = cleanupIntervalInMinutes*60*1000;
            
            Timer.Elapsed += CleanUpTokens;
            Timer.Start();
        }

        public static void Stop()
        {
            Timer.Stop();
        }

        private static void CleanUpTokens(object sender, ElapsedEventArgs e)
        {
            // Clean up expired tokens
            DateTime referenceDate = DateTime.UtcNow;

            using (var db = new CoreDbContext(_connectionString))
            {
                db.Tokens.RemoveRange(db.Tokens.Where(c => c.Expiry < referenceDate));
                db.SaveChanges();
            }
        }
    }
}
