
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DataObjects.Linq2Sql
{
    // ** Factory pattern

    // DataContext factory caches the connectionstring and the 
    // mapping source so that DataContext instances can be created quickly.
    // this significantly reduces the DataContext creation times. 
    
    public static class DataContextFactory
    {
        private static readonly string ConnectionString;
        private static readonly MappingSource MappingSource;

        // static initialization of connectionstring and mappingSource.
        // This significantly increases performance, primarily due to mappingSource cache.

        static DataContextFactory()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Action"].ConnectionString;

            var context = new ActionDataContext(ConnectionString);
            MappingSource = context.Mapping.MappingSource;
        }

        // ** Factory method. creates a new DataContext using cached connectionstring

        public static ActionDataContext CreateContext()
        {
            return new ActionDataContext(ConnectionString, MappingSource);
        }
    }
}
