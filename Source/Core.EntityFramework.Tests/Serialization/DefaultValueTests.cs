using Xunit;

namespace IdentityServer3.EntityFramework.Tests.Serialization
{
    public class DefaultValueTests
    {
        [Fact]
        public void ClientEntityShouldHaveSameDefaultsAsModel()
        {
            var model = new Core.Models.Client();
            var entity = new Entities.Client();

            Assert.Equal(model.Flow, entity.Flow);
            Assert.Equal(model.Enabled, entity.Enabled);
            Assert.Equal(model.AccessTokenLifetime, entity.AccessTokenLifetime);
            Assert.Equal(model.IdentityTokenLifetime, entity.IdentityTokenLifetime);
            Assert.Equal(model.RefreshTokenUsage, entity.RefreshTokenUsage);
            Assert.Equal(model.SlidingRefreshTokenLifetime, entity.SlidingRefreshTokenLifetime);
        }

        [Fact]
        public void ClientSecretEntityShouldHaveSameDefaultsAsModel()
        {
            var model = new Core.Models.Secret();
            var entity = new Entities.ClientSecret();

            Assert.Equal(model.Type, entity.Type);
        }

        [Fact]
        public void ScopeEntityShouldHaveSameDefaultsAsModel()
        {
            var model = new Core.Models.Scope();
            var entity = new Entities.Scope();

            Assert.Equal(model.Enabled, entity.Enabled);
        }

        [Fact]
        public void ScopeSecretEntityShouldHaveSameDefaultsAsModel()
        {
            var model = new Core.Models.Secret();
            var entity = new Entities.ScopeSecret();

            Assert.Equal(model.Type, entity.Type);
        }
    }
}
