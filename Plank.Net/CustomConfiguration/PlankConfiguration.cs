using System.Configuration;

namespace Plank.Net.CustomConfiguration
{
    public class PlankConfiguration : ConfigurationSection
    {
        #region PROPERTIES

        [ConfigurationProperty("useRedisCache", DefaultValue = false, IsRequired = false)]
        public bool UseRedisCache
        {
            get { return (bool)this["useRedisCache"]; }
        }


        #endregion

        #region METHODS

        public static PlankConfiguration GetConfiguration()
        {
            var configuration = ConfigurationManager.GetSection("plank") as PlankConfiguration;
            return configuration ?? new PlankConfiguration();
        }

        #endregion
    }
}
