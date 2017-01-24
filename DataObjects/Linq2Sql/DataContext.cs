using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Linq2Sql
{
    // ActionDataContext with the appriate connectionstring

    public class DataContext : ActionDataContext
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Action"].ConnectionString;

        // constructor
        public DataContext() : base(ConnectionString)
        {
        }
    }
}
