/*
 * FILE             : Buyer.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Data_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * The buyer represents an employee of OSHT who is tasked with requesting Customer contracts from the 
    Contract Marketplace and generating an initial Order or contract. Their chief output is an Order, which 
    is marked for action by the Planner. After the Planner’s work is completed, the Buyer confirms each 
    completed Order and generates an Invoice to the Customer.

    4.5.2.2.1 Initiates contact with the Contract Marketplace to receive contracts from Customers.

    4.5.2.2.2 Buyer may review existing Customers and accept new Customers (from the Marketplace)
    into the TMS system

    4.5.2.2.3 Buyer may initiate a new Order from the Marketplace requests.

    4.5.2.2.4 Buyer may select relevant Cities for the Order. This will nominate Carriers in those Cities 
    for Order completion (which is confirmed by the Planner)

    4.5.2.2.5 Buyer may review completed Orders and process them for Invoice Generation.

    4.5.2.2.6 Invoice Generation results in a text file being generated with appropriate billing details. 
    This information is also stored in the TMS database.
 */
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
        public Buyer()
        {

        }

        //renns
        public static void BuyerWorkFlow()
        {
            TMSDatabaseAccess tms = new TMSDatabaseAccess();
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. View Contracts from Marketplace");
                Console.WriteLine("2. View Finished Orders");
                Console.WriteLine("3. Exit");

                inp = GetInput();

                if (inp == "1")
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

                    while (inp != "Y" && inp != "N")
                    {
                        inp = GetInput();
                    }

                    if (inp == "Y")
                    {
                        Console.WriteLine("Select a Contract - (0 - {0})", mpr.Length - 1);
                        inp = GetInput();

                        int integerInp = int.Parse(inp);
                        Order o = new Order(mpr[integerInp]);
                        o.Display();
                        if (tms.InsertOrder(o))
                            Console.WriteLine("Successfully converted Marketplace Request to Order;\nDatabase insertion successful.");
                        else
                        {
                            Console.WriteLine("An error has occured -- database insertion not successful");
                            throw new Exception("TMS Database Insertion Exception");
                        }
                    }
                }
                else if (inp == "2")
                {
                    Order[] orders = tms.GetFinishedOrders();
                    if (orders == null || orders.Length == 0)
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
                else if (inp == "3")
                {

                }
            }
        }

        public MarketplaceRequest[] GetContracts()
        {
            return new ContractMarketAccess().GetAllMarketplaceRequests();
        }

        public void SelectCities(MarketplaceRequest req)
        {
            Console.WriteLine("Please select any cities that must be stopped at during this order.");
            int iter = 0;
            foreach(City c in CityList.GetList())
            {
                Console.Write("City #{0}:\t{1}", iter, c.CityName);
                if (req.CityOrigin == c.CityName)
                    Console.WriteLine("\t\t[Origin]");
                else if (req.CityDestin == c.CityName)
                    Console.WriteLine("\t\t[Destin]");
                else Console.WriteLine();
                iter++;
            }
        }
    }
}
