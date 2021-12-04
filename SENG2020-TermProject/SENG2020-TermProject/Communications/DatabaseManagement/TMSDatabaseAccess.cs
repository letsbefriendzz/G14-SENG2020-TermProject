/*
 * FILE             : TMSDatabaseAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using System;
using System.Data;
using System.Collections.Generic;
using SENG2020_TermProject.Data_Logic;
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
        public TMSDatabaseAccess()
        {
            //please please please make another account that isn't root and also host this elsewhere
            if (this.InitConnection("127.0.0.1", "3306", "root", "LocalHostRoot99", "tmsdatabase", "0"))
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
                        o.mr.ClientName, o.mr.JobType, o.mr.Quantity, o.mr.CityOrigin, o.mr.CityDestin, o.mr.VanType, o.TimeToComplete, o.DistanceToComplete, o.CostToComplete, o.OSHTSurcharge, o.IsComplete);
                }
                else
                {
                    cm.CommandText = String.Format("insert into tmsorder (ClientName, JobType, Quantity, CityOrigin, CityDestin, VanType, IsComplete)" +
                        "values (\"{0}\",{1},{2},\"{3}\",\"{4}\",{5},0);",
                        o.mr.ClientName, o.mr.JobType, o.mr.Quantity, o.mr.CityOrigin, o.mr.CityDestin, o.mr.VanType);

                }

                
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
        }

        public Order[] GetFinishedOrders()
        {
            return GetOrders("select * from tmsorder where IsComplete=1");
        }

        public Order[] GetAllOrders()
        {
            return GetOrders("select * from tmsorder");
        }

        public Order[] GetOrderByOrigin(String city)
        {
            return GetOrders("select * from tmsorder where CityOrigin=\""+city+"\"");
        }

        public Order[] GetOrderByDestin(String city)
        {
            return GetOrders("select * from tmsorder where CityDestin=\"" + city + "\"");
        }

        public Order[] GetOrderByClient(String client)
        {
            return GetOrders("select * from tmsorder where ClientName=\"" + client + "\"");
        }

        public Order[] GetIncompleteOrders()
        {
            return GetOrders("select * from tmsorder where IsComplete=0");
        }

        public bool SetOrderComplete(Order o)
        {
            int id = o.GetID();
            if(this.cn != null && this.ValidConnection)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "update tmsorder set IsComplete=1 where OrderID="+id.ToString();
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }


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
         *  I'm still working on this. Something isn't finished and I kind of forget what. It has to do with returning
         *  finished orders though. something about the order ID. come back to this later.
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

                                temp.ClientName = dr[1].ToString();
                                temp.JobType = int.Parse(dr[2].ToString());
                                temp.Quantity = int.Parse(dr[3].ToString());
                                temp.CityOrigin = dr[4].ToString();
                                temp.CityDestin = dr[5].ToString();
                                temp.VanType = int.Parse(dr[6].ToString());

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
                                    int ic = int.Parse(dr[12].ToString());
                                    bool IsComplete = false;
                                    if (ic == 0)
                                        IsComplete = false;
                                    else IsComplete = true;
                                    orders[i] = new Order(temp, (double)dr[7], int.Parse(dr[8].ToString()), (double)dr[9], (double)dr[10], IsComplete);

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
                catch (MySqlException)
                {
                    //log some stuff in here
                    return null;
                }
            }

            return orders;
        }

        public Depot[] GetDepots(String CarrierName)
        {
            Depot[] ReturnArray = null;
            if (this.cn != null && this.ValidConnection)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "select * from depot where CarrierName=\"" + CarrierName + "\"";
                        using (MySqlDataAdapter ada = new MySqlDataAdapter(cm))
                        {
                            DataTable dt = new DataTable();
                            ada.Fill(dt);

                            ReturnArray = new Depot[dt.Rows.Count];

                            int i = 0;
                            foreach(DataRow d in dt.Rows)
                            {
                                ReturnArray[0].CityName = d[0].ToString();

                                i++;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return ReturnArray;
        }
    }
}
