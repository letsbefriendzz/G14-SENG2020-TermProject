/*
 * FILE             : ContractMarketAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using MySql.Data.MySqlClient;
using SENG2020_TermProject.Data_Logic;
using System;
using System.Data;

namespace SENG2020_TermProject.DatabaseManagement
{
    static class CMDatabaseValues
    {
        public static String server = "159.89.117.198";
        public static String port = "3306";
        public static String usrnm = "DevOSHT";
        public static String pwd = "Snodgr4ss!";
        public static String table = "cmp";
        public static String ssl = "0";
    }
    /**
     * \brief       The ContractMarketAccess manages all database access logic for the TMS accessing the Contract Marketplace.
     * 
     * \details     ContractMarketAccess inherits its members from the DatabaseAccess abstract class. ContractMarketAcces includes
     *              custom methods to access the Contract Market, as well as a constructor that applies default values for Contract
     *              Market connection.
     */
    class ContractMarketAccess : DatabaseAccess
    {
        /**
         * \brief       Expanded ContractMarketAccess constructor.
         * 
         * \details     This extended ContractMarketAccess constructor accets parameters to copy into the respective
         *              fields of the ContractMarketAccess instance. if none of the values passed are invalid (ie null)
         *              the values are copied.
         */
        public ContractMarketAccess(String nserver, String nport, String nusrnm, String npwd, String ntable) : this()
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

        /**
         * \brief       Default ContractMarketAccess constructor.
         * 
         * \details     The default ContractMarketAccess constructor specifies the default values given to access
         *              the Contract Marketplace database. We have these values hardcoded.
         */
        public ContractMarketAccess()
        {
            //default contract market access values!
            this.server = CMDatabaseValues.server;
            this.port = CMDatabaseValues.port;
            this.usrnm = CMDatabaseValues.usrnm;
            this.pwd = CMDatabaseValues.pwd;
            this.table = CMDatabaseValues.table;
            initConnection(server, port, usrnm, pwd, table, "0");
        }

        /**
         * \brief   Makes an SQL request for all available contracts in the Contract Marketplace. 
         *
         * \details This function makes an SQL query to the Contract Marketplace for all available
         *          contracts. Once the query has been satisfied, the fuction creates an array of
         *          MarketplaceRequest objects to return to for further processing. If the local
         *          MySqlConnection instance is not instantiated, null is returned. If a MySqlException
         *          is caught, null is returned.
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
                            foreach (DataRow dr in dt.Rows)
                            {
                                MarketplaceRequest temp = new MarketplaceRequest();
                                //what a complete fucking hodgepodge this mess is
                                //magic numbers bad! but hey! i do not care!
                                temp.ClientName = dr[0].ToString();
                                temp.JobType = int.Parse(dr[1].ToString());
                                temp.Quantity = int.Parse(dr[2].ToString());
                                temp.CityOrigin = dr[3].ToString();
                                temp.CityDestin = dr[4].ToString();
                                temp.VanType = int.Parse(dr[5].ToString());

                                mr[MarketplaceIterator] = temp;
                                MarketplaceIterator++;
                            }
                        }
                    }
                    cn.Close();
                }
                catch (MySqlException)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
