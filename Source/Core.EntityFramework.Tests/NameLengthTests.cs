using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer3.EntityFramework.Tests
{
    public class NameLengthTests
    {
        //[Fact]
        public void NamesAreNotMoreThan30Chars()
        {
            var assembly = Assembly.GetAssembly(typeof(ClientConfigurationDbContext));
            var query =
                from t in assembly.GetTypes()
                where t.Namespace == "IdentityServer3.EntityFramework.Entities"
                select t;
            foreach(var type in query)
            {
                Assert.True(type.Name.Length <= 30, type.Name);

                foreach(var prop in type.GetProperties())
                {
                    Assert.True(prop.Name.Length <= 30, type.Name + ". " + prop.Name);
                }
            }
            
        }
    }
}
