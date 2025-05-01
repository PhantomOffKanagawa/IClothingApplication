using System;
using System.Data.Entity;

namespace IClothingApplication.Models
{
    public partial class ICLOTHINGEntities : DbContext
    {
        public ICLOTHINGEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
