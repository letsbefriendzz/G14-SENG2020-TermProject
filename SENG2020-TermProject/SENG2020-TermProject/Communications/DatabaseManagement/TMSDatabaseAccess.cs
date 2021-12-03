/*
 * FILE             : TMSDatabaseAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using System;
using System.Data;
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

        public bool InsertOrder(Order o)
        {
            cn.Open();
            using (MySqlCommand cm = this.cn.CreateCommand())
            {
                if (o.isprepped)
                {
                    cm.CommandText = String.Format("insert into tmsorder " +
                        "(ClientName, JobType, Quantity, CityOrigin, CityDestin, VanType, TimeToComplete, DistanceToComplete, CostToComplete, OSHTSurcharge, CarrierName, IsComplete) " +
                        "values (\"{0}\",{1},{2},\"{3}\",\"{4}\",{5},{6},{7},{8},{9},\"{10}\",{11});",
                        o.mr.ClientName, o.mr.JobType, o.mr.Quantity, o.mr.CityOrigin, o.mr.CityDestin, o.mr.VanType, o.TimeToComplete, o.DistanceToComplete, o.CostToComplete, o.OSHTSurcharge, o.GetCarrierName(), o.IsComplete);
                }
                else
                {
                    cm.CommandText = String.Format("insert into tmsorder (ClientName, JobType, Quantity, CityOrigin, CityDestin, VanType)" +
                        "values (\"{0}\",{1},{2},\"{3}\",\"{4}\",{5});",
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

        public Order[] GetAllOrders()
        {
            Order[] orders = null;
            if (this.cn != null && this.ValidConnection)
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand cm = cn.CreateCommand())
                    {
                        cm.CommandText = "select * from tmsorder";
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
                                    orders[i] = new Order(temp, (double)dr[7], int.Parse(dr[8].ToString()), (double)dr[9], (double)dr[10], CarrierList.GetCarrierByName(dr[11].ToString()), IsComplete);
                                    //this catastrophically bad way of parsing an sbyte to a boolean deserves to be preserved in the museum of shitty code
                                    //bool IsComplete = bool.Parse(int.Parse(dr[11].ToString()).ToString()); 
                                }
                                else
                                {
                                    orders[i] = new Order(temp);
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
    }
}
