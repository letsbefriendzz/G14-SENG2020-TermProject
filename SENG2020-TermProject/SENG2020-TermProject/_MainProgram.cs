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
using SENG2020_TermProject.UserStructure;

namespace SENG2020_TermProject
{
    class _MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.WriteLine("\n\nPress enter to continue.");
            Console.ReadLine();
        }
            
        static void Main(string[] args)
        {
            MarketplaceRequest mr = new MarketplaceRequest("Ryan's Fuckery", 0, 0, "Windsor", "Toronto", 0);
            Order o = new Order(mr);
            o.PrepOrder();
            o.Display();

            Buyer b = new Buyer();

            AnyKeyToContinue();
            return; //and then return!
        }

        static void TestCities()
        {
            foreach(City c1 in CityList.GetList())
            {
                foreach(City c2 in CityList.GetList())
                {
                    if (CarrierList.CarriersForRoute(c1, c2) == null)
                    {
                        Console.WriteLine("Bad Test");
                        Console.WriteLine("Origin:\t{0}", c1.CityName);
                        Console.WriteLine("Destin:\t{0}", c2.CityName);
                        Console.WriteLine();
                    }
                }
            }
        }

        //test harness func I'd like to hold on to
        static void PrepAndInsertOrders()
        {
            TMSDatabaseAccess tms = new TMSDatabaseAccess();

            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();

            Order[] o = new Order[mpr.Length];
            int iter = 0;
            foreach (MarketplaceRequest m in mpr)
            {
                o[iter] = new Order(m);
                //o[iter].PrepOrder(CarrierList.CarriersForRoute(o[iter])[0]);
                iter++;
            }

            foreach (Order r in o)
            {
                //Prep the order with the first carrier returned by
                //the CarriersForRoute function using itself as a parmater
                r.Display();
                tms.InsertOrder(r);
            }
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        
        //renns
        static void Buyer()
        {
            TMSDatabaseAccess tms = new TMSDatabaseAccess();
            String inp = "";
            while(inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. View Contracts from Marketplace");
                Console.WriteLine("2. View Finished Orders");
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
                        Console.WriteLine("Contract #{0}", iter);
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
                        tms.InsertOrder(o);
                    }
                }
                else if(inp == "2")
                {
                    Order[] orders = tms.GetFinishedOrders();
                    if (orders == null)
                        Console.WriteLine("No finished orders to process!");
                    else
                    {
                        foreach (Order order in orders)
                            order.Display();

                        Console.WriteLine("Generate an invoice for an order? Y/N");

                        while (inp != "Y" && inp != "N")
                        {
                            inp = GetInput();
                        }

                        if (inp == "Y")
                        {
                            Console.WriteLine("Select an Order - (0 - {0})", orders.Length - 1);
                            inp = GetInput();

                            int integerInp = int.Parse(inp);
                            Order ForInvoice = orders[integerInp];
                            if (ForInvoice.IsComplete == true)
                                Console.WriteLine("Invoice Generated");
                        }
                    }
                }
                else if(inp == "3")
                {

                }
            }
        }

        //basically just a procedural version of the planner workflow
        //this is the procedural flow in a console when a planner has
        //selected an unfilled order from the TMS database.

        //renns
        static void Planner() //this will actually take a value from a database based on user input ! :)
        {
            TMSDatabaseAccess tms = new TMSDatabaseAccess();
            Order[] orders = tms.GetIncompleteOrders();
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("1. View Unfilled Orders");
                Console.WriteLine("2. Confirm Order Completion");
                Console.WriteLine("3. Simulate Order");
                inp = GetInput();

                if(inp == "1")
                {
                    foreach (Order order in orders)
                        order.Display();
                }
                else if(inp == "2")
                {
                    int iter = 0;
                    foreach (Order or in orders)
                    {
                        Console.WriteLine("Order #{0}", iter);
                        or.Display();
                        iter++;
                    }

                    Console.WriteLine("Select an order to fulfill - (0 - {0}).", orders.Length);
                    inp = GetInput();
                    Order o = orders[int.Parse(inp)];

                    tms.SetOrderComplete(o.GetID());
                }
            }
        }

        //renns
        static String GetInput()
        {
            Console.Write(">> ");
            return Console.ReadLine();
        }
    }
}

/*
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
 */