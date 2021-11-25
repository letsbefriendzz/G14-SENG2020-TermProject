/*
 * FILE             : ContractMarketAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SENG2020_TermProject
{
    /**
     * \brief       The ContractMarketAccess manages all database access logic for the TMS accessing the Contract Marketplace.
     * 
     * \details     //
     */
    class ContractMarketAccess
    {
        private MySqlConnection cn;
        //hardcoding defaults because it's not gonna change
        //but you can change them using a nice special constructor! :)
        private String server = "159.89.117.198";
        private String port = "3306";
        private String usrnm = "DevOSHT";
        private String pwd = "Snodgr4ss!";
        private String table = "cmp";

        public ContractMarketAccess(String nserver, String nport, String nusrnm, String npwd, String ntable)
        {
            if (nserver != null && nport != null && nusrnm != null && npwd != null && ntable != null)
            {
                this.server = nserver;
                this.port = nport;
                this.usrnm = nusrnm;
                this.pwd = npwd;
                this.table = ntable;
            }
        }

        public ContractMarketAccess()
        {
            initConnection(server, port, usrnm, pwd, table, "0");
        }

        private void initConnection(String server, String port, String user, String password, String database, String sslM)
        {
            String connString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);

            try
            {
                cn = new MySqlConnection(connString);
                cn.Open();

                cn.Close();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /**
         * \brief   Makes an SQL request for all available contracts in the Contract Marketplace. 
         *
         * \details This function makes an SQL query to the Contract Marketplace for all available
         *          contracts. Once the query has been satisfied, the fuction creates an array of
         *          MarketplaceRequest objects to return to for further processing.
         *
         * \note    This function will be easily broken by a change in the Contract Marketplace database
         *          format. We are assuming that the format of (ClientName),(JobType),(Quantity),(Origins),(Destination),(VanType)
         *          will not be changing. If it does change, this function can be easily adapted - it is
         *          easily broken, however, because it references values by column index as opposed to
         *          comparing the column names with field names in the MarketplaceRequest object.
         *
         *
         * \return  MarketplaceRequest[]
         */
        public MarketplaceRequest[] GetAllMarketplaceRequests()
        {
            MarketplaceRequest[] mr = null;
            if (this.cn != null)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "select * from Contract";
                        using (MySqlDataAdapter ada = new MySqlDataAdapter(cm))
                        {
                            DataTable dt = new DataTable();
                            ada.Fill(dt);

                            //we could really just use a dictionary in the MarketplaceRequest object if we
                            //wanted maximum modularity, but we're assuming that the Contract table doesn't
                            //radically change at any point. If it were to radically change, this code section
                            //needs to be adapted, which isn't too hard.

                            mr = new MarketplaceRequest[dt.Rows.Count];
                            int MarketplaceIterator = 0;
                            foreach(DataRow dr in dt.Rows)
                            {
                                MarketplaceRequest temp = new MarketplaceRequest();
                                //what a complete fucking hodgepodge this mess is
                                //magic numbers bad! but hey! i do not care!
                                temp.ClientName  = dr[0].ToString();
                                temp.JobType     = int.Parse(dr[1].ToString());
                                temp.Quantity    = int.Parse(dr[2].ToString());
                                temp.CityOrigin  = dr[3].ToString();
                                temp.CityDestin  = dr[4].ToString();
                                temp.VanType     = int.Parse(dr[5].ToString());

                                mr[MarketplaceIterator] = temp;
                                MarketplaceIterator++;
                            }
                        }
                    }
                    cn.Close();
                }
                catch(MySqlException e)
                {

                }
            }

            return mr;
        }
    }
}
