using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.DataPersistence
{
    /// <summary>
    /// DB Context for SQL LocalDB connection
    /// </summary>
    public class FootlooseFSSQLLocalDBContext : FootlooseFSDBContext
    {
        public FootlooseFSSQLLocalDBContext() : base()
        {
            // Setup the data directory for the SQL Server LocalDB connection which references 
            // DataDirectory in the path of the MDF file
            var dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
        }
    }
}
