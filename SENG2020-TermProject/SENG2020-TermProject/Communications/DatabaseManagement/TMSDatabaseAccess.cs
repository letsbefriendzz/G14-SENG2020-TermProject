/*
 * FILE             : TMSDatabaseAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  The TMSDatabaseAccess class provides an interface with the TMSDatabase. It inherits
 *  its SQL connection abilities from the DatabaseAccess class.
 *  
 *  The TMSDatabaseAccess class provides methods for retrieving sets of orders based
 *  on a variety of parameters. It also provides an interface for updating an order
 *  to a finished state.
 */

using System;
using System.Data;
using SENG2020_TermProject.Data_Logic;
using SENG2020_TermProject.Communications;
using MySql.Data.MySqlClient;

namespace SENG2020_TermProject.DatabaseManagement
{
    /**
     * \brief       The TMSDatabaseAccess class manages all database access logic for the TMS accessing the local TMS database.
     * 
     * \details     //
     */
    class TMSDatabaseAccess : DatabaseAccess
    {
        public TMSDatabaseAccess(String password)
        { //LocalHostRoot99
            //please please please make another account that isn't root and also host this elsewhere
            if (this.InitConnection("127.0.0.1", "3306", "root", password, "tmsdatabase", "0"))
                this.ValidConnection = true; //LocalHostRoot99
        }

        //Constructor that uses a usr and pwd string for user and password.
        public TMSDatabaseAccess(String usr, String pwd)
        {
            if (this.InitConnection("127.0.0.1", "3306", usr, pwd, "tmsdatabase", "0"))
                this.ValidConnection = true;
        }

        /*
         * NAME : InsertOrder
         * DESC :
         *  This function inserts an order based on parameters passed to it, in the form of an Order object.
         *  If the Order isprepped flag is set, this means that cost, distance, and surcharges have been calculated.
         *  These values can thus be inserted into the database. This should also mean that the order is finished;
         *  currently there is no use case where a prepped order wouldn't be finished, as the Planner must prep
         *  the order before simulating time, whereby the order is considered finished.
         *  
         *  If the order is not prepped, we are inserting only the MarketplaceRequest fields that the Order contains.
         *  This is use case is used by the Buyer when inserting a newly created order based on a MarketplaceRequest.
         *  
         *  Finally, if an error occurs, false is returned. If the insertion is successful, true is returned.
         * RTRN : bool
         * PARM : Order
         */
        public bool InsertOrder(Order o)
        {
            cn.Open();
            using (MySqlCommand cm = this.cn.CreateCommand())
            {
                if (o.isprepped)
                {
                    cm.CommandText = String.Format("insert into tmsorder " +
                        "(ClientName, JobType, Quantity, CityOrigin, CityDestin, VanType, TimeToComplete, DistanceToComplete, CostToComplete, OSHTSurcharge, IsComplete) " +
                        "values (\"{0}\",{1},{2},\"{3}\",\"{4}\",{5},{6},{7},{8},{9},\"{10}\",{11});",
                        o.mr.GetClientName(), o.mr.GetJobType(), o.mr.GetQuantity(), o.mr.GetCityOrigin(), o.mr.GetCityDestination(), o.mr.GetVanType(), o.GetTimeToComplete(), o.GetDistanceToComplete(), o.GetCostToComplete(), o.GetOSHTSurcharge(), o.GetIsComplete());
                }
                else
                {
                    cm.CommandText = String.Format("insert into tmsorder (ClientName, JobType, Quantity, CityOrigin, CityDestin, VanType, IsComplete)" +
                        "values (\"{0}\",{1},{2},\"{3}\",\"{4}\",{5},0);",
                        o.mr.GetClientName(), o.mr.GetJobType(), o.mr.GetQuantity(), o.mr.GetCityOrigin(), o.mr.GetCityDestination(), o.mr.GetVanType());

                }

                try
                {
                    if (cm.ExecuteNonQuery() != 0)
                    {
                        cn.Close();
                        return true;
                    }
                    else
                    {
                        cn.Close();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    FileAccess.Log("Error in TMSDatabaseAccess : InsertOrder\n" +  e.ToString());
                    cn.Close();
                    return false;
                }
            }
        }

        #region GetOrder Extensions

        /*
         * NAME(S)  : Get{xyz}Order
         * DESC     :
         *  The following functions don't warrant their own individual headers. Each of them,
         *  based on a condition defined clearly in their method title, make a specific query
         *  to the GetOrders() function. Some are hardcoded, some allow a single string input
         *  for a certain field to be queried for.
         * RTRN     : Order[]
         * PARM     : String or void
         */

        //Extension of GetOrders()
        public Order[] GetFinishedOrders()
        {
            return GetOrders("select * from tmsorder where IsComplete=1");
        }

        //Extension of GetOrders()
        public Order[] GetAllOrders()
        {
            return GetOrders("select * from tmsorder");
        }

        //Extension of GetOrders()
        public Order[] GetOrderByOrigin(String city)
        {
            return GetOrders("select * from tmsorder where CityOrigin=\"" + city + "\"");
        }

        //Extension of GetOrders()
        public Order[] GetOrderByDestin(String city)
        {
            return GetOrders("select * from tmsorder where CityDestin=\"" + city + "\"");
        }

        //Extension of GetOrders()
        public Order[] GetOrderByClient(String client)
        {
            return GetOrders("select * from tmsorder where ClientName=\"" + client + "\"");
        }

        //Extension of GetOrders()
        public Order[] GetIncompleteOrders()
        {
            return GetOrders("select * from tmsorder where IsComplete=0");
        }

        #endregion

        /*
         * NAME : GetOrder
         * DESC :
         *  This function makes an SQL request to the TMS Databse based on the command text given to it. This function is
         *  private because it is intended exclusively to return Order objects; if it was a general command interface, we
         *  could make it public.
         *  
         *  This function is referenced by a bunch of subfunctions of the TMSDatabaseAccess class that pass hardcoded SQL
         *  queries based on their own parameters. For Example, GetOrderByClient accepts a string that is expected to be
         *  a Client name. The ClientName parameter is then concatenated to the commandText string to form a proper SQL
         *  request.
         *  
         *  This function creates an array of Orders sized to however many rows were returned in the DataTable that the
         *  SQL query populates. Then, for each row returned, a new MarketplaceRequest object is created and popualted
         *  with the respective values. An Order object is then instantiated in the array using this MarketplaceRequest
         *  instance, until the Order array is fully populated by unfilled, unprepped orders.
         *  
         *  In the case that there are finished orders in the request, these will be returned as well; the Order instance
         *  in the array is instantiated with the MarketplaceRequest, as well as the rest of the values that are expected
         *  to be returned. This includes the time to complete, distance, carrier cost, OSHT surcharge, and completion status.
         *  
         *  
         *  - Ryan Enns, 2021-12-03 - code written probably like three days ago at this point? comment written on given date
         * RTRN
         * PARM
         */
        private Order[] GetOrders(String commandText)
        {
            Order[] orders = null;
            if (this.cn != null && this.ValidConnection)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = commandText;
                        using (MySqlDataAdapter ada = new MySqlDataAdapter(cm))
                        {
                            DataTable dt = new DataTable();
                            ada.Fill(dt);

                            //once again, this is a mediocre way of doing this!
                            //if you can't tell, I just cloned the ContractMarketAccess code and adapted it for
                            //order objects! Wow!
                            orders = new Order[dt.Rows.Count];
                            int i = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                //temp mplace req so we can init an order with it
                                MarketplaceRequest temp = new MarketplaceRequest();

                                temp.SetClientName(dr[1].ToString());
                                temp.SetJobType(int.Parse(dr[2].ToString()));
                                temp.SetQuantity(int.Parse(dr[3].ToString()));
                                temp.SetCityOrigin(dr[4].ToString());
                                temp.SetCityDestination(dr[5].ToString());
                                temp.SetVanType(int.Parse(dr[6].ToString()));
                                int.Parse(dr[6].ToString());

                                if (!(dr[7].GetType().ToString() == "System.DBNull"))
                                {
                                    /*
                                    happy parsing methods for dbase values! :)

                                    double Time = (double)dr[7];
                                    int Distance = int.Parse(dr[8].ToString());
                                    double Cost = (double)dr[9];
                                    double OSTHCharge = (double)dr[10];
                                    */

                                    //gotta pre-parse this bad boy to an integer and then logically determine the bool val from it because
                                    //csharp has a personal vendetta against redheads like myself
                                    int ic = int.Parse(dr[11].ToString());
                                    bool IsComplete = false;
                                    if (ic == 0)
                                        IsComplete = false;
                                    else IsComplete = true;
                                    orders[i] = new Order(int.Parse(dr[0].ToString()), temp, (double)dr[7], int.Parse(dr[8].ToString()), (double)dr[9], (double)dr[10], IsComplete);

                                    //this catastrophically bad way of parsing an sbyte to a boolean deserves to be preserved in the museum of awful code
                                    //bool IsComplete = bool.Parse(int.Parse(dr[11].ToString()).ToString()); 
                                }
                                else
                                {
                                    orders[i] = new Order(int.Parse(dr[0].ToString()), temp);
                                }
                                i++;
                            }
                        }
                    }
                    cn.Close();
                }
                catch (MySqlException mse)
                {
                    FileAccess.Log("Error in TMSDatabaseAccess.cs :: Getorders(String commandText)\n " + mse.ToString());
                    return null;
                }
            }

            return orders;
        }

        /*
         * NAME : SetOrderComplete
         * DESC :
         *  When the Planner is finished prepping and simulating an Order, it needs to be updated
         *  in the TMS database. An unprepped order has half of its fields set as null; once it is
         *  prepped and finished, the remaining fields (time, distance, cost, osht charges) are
         *  assigned values and the IsComplete flag is set to 1.
         *  
         *  This function accepts an Order object that has been retrieved from the db already. This
         *  can be identified by the Order having a set ID field. If the Order ID field is -1, this
         *  is a fresh that we're attempting to udpate - in other words, it won't exist in the db.
         *  If this is the case, we throw an exception.
         *  
         *  If the Order has been retrieved from the DB and has a valid ID, we can continue to update
         *  it. This involves an update SQL query 
         * RTRN
         * PARM
         */
        public bool SetOrderComplete(Order o)
        {
            int id = o.GetID();
            if (id == -1) throw new Exception("Non DB Order passed to SetOrderCompelte");

            if (this.cn != null && this.ValidConnection)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "update tmsorder set TimeToComplete=" + o.GetTimeToComplete() + ", DistanceToComplete=" + o.GetDistanceToComplete() +
                                         ", CostToComplete=" + o.GetCostToComplete() + ", OSHTSurcharge=" + o.GetOSHTSurcharge() + ", IsComplete=1 " +
                                          "where OrderID=" + id.ToString();
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    FileAccess.Log(String.Format("ERROR in TMSDatabaseAccess.cs :: SetorderComplete(Order o)\n {0}", e.ToString()));
                    cn.Close();
                    return false;
                }
            }
            return false;
        }
    }
}
