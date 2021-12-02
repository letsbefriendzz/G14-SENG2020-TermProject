/*
 * FILE             : _MainProgram.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */


//makes sense to define the DOxygen index stuff where main() can be found.
/**
 * \mainpage Transportation Management System
 * 
 * \section         Functional      Functional Requirements
 * 
 * The OSHT Transportation Management System must allow various user types to manage the logistics
 * of shipping sequences between various Ontario cities. This is achieved through accessing a Contract
 * Marketplace, where valid carrier information is then presented. The TMS user can then choose the
 * carrier they'd like, simulate passing time, and then declare an order fulfilled. An invoice can
 * be generated and the Order is updated.
 * 
 * \section         ToDoList        To Do list
 * 
 * \subsection      Configure Carrier Update System
 * 
 * What is it? How does it work? Is it just Carriers.csv with us reading and writing to the file based
 * on orders we take and complete?
 * 
 * \subsection      Multi-Carrier Orders
 * 
 * Can an order be satisfied by multiple carriers? If so, does it require that the product be dropped
 * at the respective carrier's closest depot, only to then be picked up by another carrier to its final
 * destination?
 * <br></br>
 * What do we do with orders that can't be satisfied by a single carrier service?
 * 
 * \subsection      Start Writing WPF UI
 * 
 * For the functioning prototype, I (Ryan) will be making a console UI to work with. However, requirements
 * state that a WPF GUI is required - this needs to be created. However, it should be relatively easy to
 * snap this on top of the functioning program if a functioning console UI is already largely working.
 * 
 * \section         Database        Database Requirements
 * 
 * There are 3 databases accessed by the TMS; the Contract Marketplace, the Carriers Update System
 * flatfile, and the local TMS database.
 * 
 * \subsection  Contract Marketplace
 * 
 * The Contract Marketplace is accessed by the ContractMarketAccess class, a class that extends the
 * abstract DatabaseAccess class. The ContractMarketAccess aims to provide an easy to use interface
 * for the Buyer users to request varying types of Contracts. This includes getting just LTL or FTL
 * contracts, Reefer contracts, contracts based on location, or just getting all available contracts.
 * 
 * \subsection  Carrier Update System
 * 
 * How exactly this works I don't know. note to self -- ask laura lol
 * 
 * \subsection  Local TMS Database
 * 
 * The local TMS database stores information about customers, open and closed orders, employees, and
 * other data.
 */

//here is the SET file header template:
/*
 * FILE             :
 * PROJECT          :
 * PROGRAMMER       :
 * FIRST VERSION    :
 * DESCRIPTION      :
 */

using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Data_Logic;
using System;
using System.Collections.Generic;

namespace SENG2020_TermProject
{
    class _MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.ReadLine();
        }


        //what is this doing?
        //lol
        static void Main(string[] args)
        {
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();

            List<Order> o = new List<Order>();

            bool exit = false;
            while(!exit)
            {
                Order or = Buyer();
                if (or == null)
                    exit = true;
                else o.Add(or);
            }

            //Planner(new Order(mpr[3]));

            AnyKeyToContinue();
            return; //and then return!
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        static Order Buyer()
        {
            String inp = "";
            while(inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. View Contracts from Marketplace");
                Console.WriteLine("2. View Unfilled Orders");
                Console.WriteLine("3. Exit");

                inp = GetInput();

                if(inp == "1")
                {
                    inp = "";
                    ContractMarketAccess cma = new ContractMarketAccess();
                    MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
                    int iter = 0;
                    foreach (MarketplaceRequest m in mpr)
                    {
                        Console.WriteLine("Order #{0}", iter);
                        m.Display();
                        iter++;
                    }

                    Console.WriteLine("Select From Contracts? Y/N");

                    while(inp != "Y" && inp != "N")
                    {
                        inp = GetInput();
                    }

                    if(inp == "Y")
                    {
                        Console.WriteLine("Select a Contract - (0 - {0})", mpr.Length - 1);
                        inp = GetInput();

                        int integerInp = int.Parse(inp);
                        Order o = new Order(mpr[integerInp]);
                        o.Display();
                        return o;
                    }
                }
                else if(inp == "2")
                {

                }
                else if(inp == "3")
                {
                    return null;
                }
            }
            return null;
        }

        //basically just a procedural version of the planner workflow
        //this is the procedural flow in a console when a planner has
        //selected an unfilled order from the TMS database.
        static void Planner(Order o) //this will actually take a value from a database based on user input ! :)
        {
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. Select a Carrier for Order");
                if(o.isprepped) Console.WriteLine("2. Simulate Time");

                inp = GetInput();

                if (inp == "1")
                {
                    if (CarrierList.CarriersForRoute(o) != null)
                    {
                        Console.WriteLine("Order Details:");
                        o.Display();
                        List<Carrier> c = CarrierList.CarriersForRoute(o);
                        Console.WriteLine("Available Carriers to Fulfill Order:");
                        foreach (Carrier ca in c)
                        {
                            ca.Display();
                        }

                        Console.WriteLine("Select a Carrier - (0 - {0})", c.Count - 1);
                        inp = GetInput();

                        int integerInp = int.Parse(inp);
                        o.PrepOrder(c[integerInp]);

                        o.Display();
                    }
                    else
                    {
                        Console.WriteLine("There are no Carrier services that can fulfill this order!");
                        Console.WriteLine("Order Details:");
                        o.Display();
                    }
                }
                else if (inp == "2")
                {

                }
                else if (inp == "3")
                {

                }
            }
        }

        static String GetInput()
        {
            Console.Write(">> ");
            return Console.ReadLine();
        }
    }
}

/*
            String c1 = "Windsor";
            String c2 = "Ottawa";
            Console.WriteLine("Getting distance between {0} and {1}.\nDistance:\t{2}", c1, c2, CityList.DrivingDistance(c1,c2));
            Console.WriteLine("Getting driving time between {0} and {1}.\nTime:\t\t{2}", c1, c2, CityList.DrivingTime(c1, c2));

            Console.WriteLine("Stops between {0} and {1}:\t{2}",c1,c2, CityList.LTLStops(c1, c2));
            //good question!
            //we're making a new cma object and getting all available requests from the marketplace database.
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
            //and for each one we receive, we dump out the contents
            if(mpr != null)
            {
                foreach (MarketplaceRequest mr in mpr)
                    mr.Display();
            }

            //then we get any key to continue
            AnyKeyToContinue();

            CityList.DisplayList();

            AnyKeyToContinue();
 */