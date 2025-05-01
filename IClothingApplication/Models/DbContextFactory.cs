using System;
using System.Configuration;

namespace IClothingApplication.Models
{
    public static class DbContextFactory
    {
        public static ICLOTHINGEntities Create()
        {
            var configConnectionString = ConfigurationManager.ConnectionStrings["ICLOTHINGEntities"]?.ConnectionString;
            if (!string.IsNullOrEmpty(configConnectionString))
            {
                return new ICLOTHINGEntities(configConnectionString);
            }

            var envConnectionString = Environment.GetEnvironmentVariable("ICLOTHING_DB_CONN");
            return !string.IsNullOrEmpty(envConnectionString)
                ? new ICLOTHINGEntities(envConnectionString)
                : new ICLOTHINGEntities();
        }
    }
}
