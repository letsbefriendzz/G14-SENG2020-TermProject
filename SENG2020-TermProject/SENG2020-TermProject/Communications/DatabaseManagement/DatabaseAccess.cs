/*
 * FILE             : DatabaseAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER       : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  The DatabaseAccess class is an abstract class from which specific db connection
 *  interface classes are derived. The class contains protected fields for the server
 *  IP, Port, Username, Password, relation, and local mySqlConnection instance.
 *  
 *  The InitConnection method accepts parameters for each of the aforementioned
 *  fields and attempts to connect to a SQL database using the fields provided.
 */

using MySql.Data.MySqlClient;
using SENG2020_TermProject.Communications;
using System;

namespace SENG2020_TermProject.DatabaseManagement
{
    /**
     * \brief       The DatabaseAccess class is an abstract class that supplies the basics needed for connecting to an SQL database.
     * 
     * \details     DatabaseAccess is the parent class of ContractMarketAccess and TMSDatabaseAccess. The DatabaseAccess class abstracts
     *              from each of these whatever shared attributes they have. This includes SQL server details like the server and port,
     *              as well as the username and password that we're trying to access the database with.
     */
    public abstract class DatabaseAccess
    {
        /// \brief      The object that actually connects us to an SQL database.
        protected MySqlConnection cn;
        /// \brief      Represents the IP address of the SQL server we're connecting to.
        protected String server;
        /// \brief      Represents the port of the SQL server we're conencting to.
        protected String port;
        /// \brief      Represents the username of the account we're signing into on the SQL server.
        protected String usrnm;
        /// \brief      Represents the password of the account we're signing into on the SQL server.
        protected String pwd;
        /// \brief      Represents the schema in which we want access to in the database.
        protected String table;

        public bool ValidConnection = false;

        public DatabaseAccess()
        {
            cn = null;
            server = null;
            port = null;
            usrnm = null;
            pwd = null;
            table = null;
        }

        /**
         *  \brief      Initializes the MySqlConnection object and tests a connection to it.
         *  
         *  \details    This function initializes the local MySqlConnection member using the parameters passed to it.
         *              It then test the connection to the server specified; if an exception occurs, we dump the information
         *              to the console and revert the MySqlConnection instance to null.
         */
        protected bool InitConnection(String server, String port, String user, String password, String database, String sslM)
        {
            String connString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);

            try
            {
                cn = new MySqlConnection(connString);
                cn.Open();
                cn.Close();
                return true;
            }
            catch (Exception mse)
            {
                FileAccess.Log("ERROR in DatabaseAccess.cs :: InitConnection\n" + mse.ToString());
                this.ValidConnection = false;
                cn = null;
                return false;
            }
        }
    }
}
