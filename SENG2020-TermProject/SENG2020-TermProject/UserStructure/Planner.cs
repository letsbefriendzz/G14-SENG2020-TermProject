/*
 * FILE             : Planner.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using System;
using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Data_Logic;

/*
The Planner employee is responsible for furthering the order by selecting one or more registered 
Carriers to fulfill the Order, in the form of Trips. Once assigned, the Planner monitors the progression of
time in the application, ensuring that when all Trips on an order are completed, the Order will be 
marked as Completed and sent back to the Buyer for Invoice Generation. Finally, the Planner may also 
produce reports showing aggregate activity in OSHT.

4.5.2.3.1 Planner receives Orders from the Buyer.

-- Satisfied via TMSDatabaseAccess. Instead of passing between class instances, we pass order
instances to the database. This enables async access of order resources, rather than creating a
dependency on Buyer and Planner users being active simultaneously.

4.5.2.3.2 Planner selects Carriers from the targeted cities to complete the Order, which adds a 
‘Trip’ to the Order for each Carrier selected

-- Partially satisfied -- currently the CarrierList.CarriersForRoute(Order o) function returns a
list of Trip instances that will satisfy the order. This is the only option available to the Planner.
To better meet requirements, return multiple available options and allow the carrier to choose - in
spite of cost or time efficiency. Give the user choice.

4.5.2.3.3 Carriers may be limited in their transportation capacity, thus the Planner ensures that 
multiple Trips, if necessary are attached to the Order.

-- This requirement is satisfied by the CarriersForRoute function. A trip is not deemd possible if the
depot for a given carrier in a given city does not have capacity to fulfill it. Thus the Planner does
not have to intentionally check if availability is present; the OSHT TMS system performs this automatically.

4.5.2.3.4 The Planner may simulate the passage of time in 1-day increments in order to mover 
Orders and their trips to completed state

-- UNFINISHED

4.5.2.3.5 Planner may confirm an order is completed. Completed Orders are marked for follow-up 
from the Buyer

-- UNFINISHED

4.5.2.3.6 Planner may see a summary of all active Orders in a status screen.

-- Satisfied to my best understanding; all unfinished orders can be retrieved from the TMS database and
given to the user.

4.5.2.3.7 (Optional) The status screen is highly graphical, perhaps using some kind of Google Maps 
plugin.

-- Sorry Laura, but it's the 4th, I'm busy on the 7th, so I need this done for noon of the 7th at the
latest. No chance here. Maybe if my group members had done anything at all at this point, we'd be in a
different position.

4.5.2.3.8 Planner may generate a summary report of all Invoice data for a) all time, and b) The 
‘past 2 weeks’ of simulated time
 */
namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       Defines the permissions and abilities of the Planner user type.
     * 
     * \details     The Planner is responsible for furthering created orders by selecting
     *              a carrier or carriers to fulfill it. The planner also keeps track of
     *              order progress.
     */
    class Planner : User
    {
        private static void PlannerHeader()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("Logged in as: Planner");
            Console.WriteLine("=====================");
        }

        public void DisplayUnfilledOrders()
        {
            Console.WriteLine("\n\nTMS DATABASE - UNFINISHED ORDERS");
            Console.WriteLine(System.DateTime.Now);
            Console.WriteLine("===================================\n");
            Order[] orders = tms.GetIncompleteOrders();
            int iter = 0;
            foreach (Order order in orders)
            {
                Console.WriteLine("Order #{0}", iter);
                order.Display();
                iter++;
            }
        }

        //renns
        public void PlannerWorkFlow() //this will actually take a value from a database based on user input ! :)
        {
            PlannerHeader();
            if (!this.tms.ValidConnection) return; // exit the application immediately upon establishing the connection is invalid

            Order[] orders;
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("1. View Unfilled Orders");
                Console.WriteLine("2. Prepare and Simualte an Order");
                inp = GetInput();

                if (inp == "1")
                {
                    DisplayUnfilledOrders();
                }
                else if (inp == "2")
                {
                    orders = tms.GetIncompleteOrders();
                    DisplayUnfilledOrders();

                    Console.WriteLine("Select an order to fulfill - (0 - {0}).", orders.Length - 1);
                    Order o = orders[GetIntBetween(orders.Length - 1, 0)];
                    o.PrepOrder();
                    o.Display();

                    if (tms.SetOrderComplete(o))
                    {
                        Console.WriteLine("Sucessfully finished Order #{0} - {1}", o.GetID(), o.mr.ClientName);
                    }
                    else
                    {
                        Console.WriteLine("An error occured in TMSDatabaseAccess.cs - See Logs");
                    }
                }
            }
        }
    }
}
