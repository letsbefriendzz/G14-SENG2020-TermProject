using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SENG2020_TermProject.DatabaseManagement
{
    /**
     * \brief       The DatabaseAccess class is an abstract class that supplies the basics needed for connecting to an SQL database.
     * 
     * \details     //
     */
    abstract class DatabaseAccess
    {
        protected MySqlConnection cn;
        //hardcoding defaults because it's not gonna change
        //but you can change them using a nice special constructor! :)
        protected String server;
        protected String port;
        protected String usrnm;
        protected String pwd;
        protected String table;

        public DatabaseAccess()
        {
            cn = null;
            server = null;
            port = null;
            usrnm = null;
            pwd = null;
            table = null;
        }
    }
}
