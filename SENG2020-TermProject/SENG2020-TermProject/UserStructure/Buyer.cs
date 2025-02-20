﻿/*
 * FILE             : Buyer.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  Buyer.cs outlines all logic pertaining the console UI workflow of the Buyer. Inheriting from
 *  the User class, the Buyer contains various console IO functions and mainly acts as the supplier
 *  for a console UI when logged in as a Buyer instance.
 *  
 *  The complete list of Buyer requirements, as found in the TMS Project Overview, are found below.
 *  Along with this, underneath each requirement, I describe how I have either met the requirement,
 *  met a modified or improved version of the requirement, or clarify that the requirement has not 
 *  been met.
 */

/*
The buyer represents an employee of OSHT who is tasked with requesting Customer contracts from the 
Contract Marketplace and generating an initial Order or contract. Their chief output is an Order, which 
is marked for action by the Planner. After the Planner’s work is completed, the Buyer confirms each 
completed Order and generates an Invoice to the Customer.

    4.5.2.2.1 Initiates contact with the Contract Marketplace to receive contracts from Customers.

    -- Satisfied - the Buyer can access and have displayed an array of MarketplaceRequest objects
    made from parsed SQL db values.

4.5.2.2.2 Buyer may review existing Customers and accept new Customers (from the Marketplace)
into the TMS system

N/A

    4.5.2.2.3 Buyer may initiate a new Order from the Marketplace requests.

    -- Satisfied, the user can init a new Order object from Marketplace Requests and can insert this order
    into the TMS database.

4.5.2.2.4 Buyer may select relevant Cities for the Order. This will nominate Carriers in those Cities 
for Order completion (which is confirmed by the Planner)

-- Not satisfied; rendered moot by this implementation. The Buyer does not need to manually choose cities
to add to an order to accomodate for a failure to find carriers. This TMS accepts an origin and destination
city, and based on available carriers returns a carrier or group of carriers that can satisfy the route.

    4.5.2.2.5 Buyer may review completed Orders and process them for Invoice Generation.

    -- Partially satisfied - the Buyer can currently access the order db and get a list of finished orders.
    Currently lacking file system access and parsing to an invoice.

4.5.2.2.6 Invoice Generation results in a text file being generated with appropriate billing details. 
This information is also stored in the TMS database.
 */

using SENG2020_TermProject.Communications;
using SENG2020_TermProject.Data_Logic;
using SENG2020_TermProject.DatabaseManagement;
using System;

namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       Defines the permissions and abilities of the Buyer user type.
     * 
     * \details     The Buyer is an OSHT employee who is responsible for retrieving
     *              contracts from the marketplace and generating unfilled orders for
     *              the Planner to further process. The Buyer, once an order has been
     *              fulfilled, will generate an invoice for the customer and the TMS
     *              database.
     */
    class Buyer : User
    {
        //Displays a console Buyer header.
        private static void BuyerHeader()
        {
            Console.WriteLine("===================");
            Console.WriteLine("Logged in as: Buyer");
            Console.WriteLine("===================");
        }

        /*
         * NAME : DisplayAllContracts
         * DESC :
         *  Makes a request to a local ContractMarketAccess instance.
         *  Iterates through the array of MarketplaceRequests that is
         *  returned and displays each in the console.
         * RTRN : //
         * PARM : //
         */
        private void DisplayAllContracts()
        {
            Console.WriteLine("\n\nCONTRACT MARKET PLACE CONTRACTS");
            Console.WriteLine(System.DateTime.Now);
            Console.WriteLine("===============================\n");
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
            int iter = 0;
            foreach (MarketplaceRequest m in mpr)
            {
                Console.WriteLine("Contract #{0}", iter);
                m.Display();
                iter++;
            }
            return;
        }

        /*
         * NAME : DisplayFinishedOrders
         * DESC :
         *  Displays all finished orders found in the TMS Database.
         * RTRN : //
         * PARM : //
         */
        private void DisplayFinishedOrders()
        {
            Order[] orders = tms.GetFinishedOrders();

            Console.WriteLine("\n\nTMS DATABASE - FINISHED ORDERS");
            Console.WriteLine(System.DateTime.Now);
            Console.WriteLine("==============================\n");
            if (orders == null || orders.Length == 0)
                Console.WriteLine("No finished orders to process!");
            else
            {
                int iter = 0;
                foreach (Order order in orders)
                {
                    Console.WriteLine("Order #{0}", iter);
                    order.Display();
                    iter++;
                }
            }
        }

        /*
         * NAME : GetDatabaseAccess
         * DESC :
         *  Prompts the user for the password for the respective db account they're
         *  attempting to log in with. inits the local TMSDatabaseAccess instance with
         *  the password.
         * RTRN : //
         * PARM : //
         */
        private void GetDatabaseAccess()
        {
            if (this.tms != null) return;

            Console.WriteLine("Enter the Buyer TMS Database password: ");
            tms = new TMSDatabaseAccess("buyer", GetInput(), "tmsorder");
        }

        /*
         * NAME : BuyerWorkFlow
         * DESC :
         *  The main user flow of the Buyer. See requirements above for
         *  better understanding of it.
         * RTRN : //
         * PARM : //
         */
        public void BuyerWorkFlow()
        {
            BuyerHeader();
            GetDatabaseAccess();
            Console.WriteLine("\n\n");
            if (!this.tms.ValidConnection)
            {
                Console.WriteLine("Invalid TMS Database username or password!");
                FileAccess.Log("Invalid Login Attempt by user Buyer");
                return;
            } // exit the application immediately upon establishing the connection is invalid
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. View Contracts from Marketplace");
                Console.WriteLine("2. Generate Order from Contract");
                Console.WriteLine("3. View Finished Orders");
                Console.WriteLine("4. Generate Invoice");
                Console.WriteLine("5. Insert Customer Order");
                Console.WriteLine("0. Exit");

                inp = GetInput();

                Delay();
                if (inp == "1")
                {
                    
                    inp = "";
                    DisplayAllContracts();
                }
                else if (inp == "2")
                {
                    DisplayAllContracts();
                    ContractMarketAccess cma = new ContractMarketAccess();
                    MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
                    Console.WriteLine("Select a Contract - (0 - {0})", mpr.Length - 1);

                    Order o = new Order(mpr[GetIntBetween(mpr.Length - 1, 0)]);
                    o.Display();
                    Console.WriteLine("Insert this Order into the TMS Database? Y/N");
                    inp = GetYesNo();
                    if(inp == "Y")
                    {
                        if (tms.InsertOrder(o))
                        {
                            Console.WriteLine("Successfully converted Marketplace Request to Order;\nDatabase insertion successful.\n");
                        }
                        else
                        {
                            Console.WriteLine("An error has occured -- database insertion not successful");
                            throw new Exception("TMS Database Insertion Exception");
                        }
                    }
                }
                else if (inp == "3")
                {
                    DisplayFinishedOrders();
                }
                else if (inp == "4")
                {
                    DisplayFinishedOrders();
                    Order[] orders = tms.GetFinishedOrders();

                    if (!(orders.Length == 0))
                    {
                        Console.WriteLine("Select an Order - (0 - {0})", orders.Length - 1);
                        inp = GetInput();

                        int integerInp = int.Parse(inp);
                        Order ForInvoice = orders[integerInp];
                        if (ForInvoice.GetIsComplete() == true)
                        {
                            Console.WriteLine("Invoice Generated");
                        }

                        Console.WriteLine("Invoice can be found at:\n{0}", FileAccess.CreateInvoice(ForInvoice));
                    }
                }
                else if (inp == "5")
                {
                    MarketplaceRequest mr = new MarketplaceRequest();
                    Console.WriteLine("Client Name:");
                    mr.SetClientName(GetInput());
                    Console.WriteLine("Job Type (1/0):");
                    mr.SetJobType(GetIntBetween(1, 0));
                    if (mr.GetJobType() == 1)
                    {
                        Console.WriteLine("Pallet Quantity: ");
                        mr.SetQuantity(GetIntBetween(24, 0));
                    }
                    Console.WriteLine("Origin City:");
                    mr.SetCityOrigin(GetCityName());
                    Console.WriteLine("Destination City:");
                    mr.SetCityDestination(GetCityName());
                    Console.WriteLine("Van Type (1/0):");
                    mr.SetVanType(GetIntBetween(1, 0));

                    Order o = new Order(mr);
                    o.Display();

                    Console.WriteLine("Insert this order into TMS Database? (Y/N)");

                    inp = GetYesNo();
                    if (inp == "Y")
                    {
                        if (this.tms.InsertOrder(o))
                        {
                            Console.WriteLine("Successfully inserted order into DB");
                        }
                        else
                        {
                            Console.WriteLine("An error occured during db insertion; please see log files for details.");
                        }
                    }
                }
                else if (inp == "0")
                {
                    inp = null;
                }
                Console.WriteLine();
            }
        }
    }
}
