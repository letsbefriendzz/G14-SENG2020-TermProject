/*
 * FILE             : Planner.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  The Planner.cs file outlines all logic pertaining to a Console implementation of a Planner
 *  user's workflow. The Planner class inherits from the User class, and contains various functions
 *  for the user to provide db login information and perform the various tasks that the Planner
 *  is intended to be able to complete. The complete list of Planner requirements, as found in
 *  the TMS Project Overview, are found below. Along with this, underneath each requirement,
 *  I describe how I have either met the requirement, met a modified or improved version of the
 *  requirement, or clarify that the requirement has not been met.
 */

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

    -- Mostly satisfied. The TMS system returns a list of potential routes that can be used to fulfill
    an order. The Planner can then select the route, which already contains nominated Carriers, to
    fill the order. The system prioritizes single Carrier routes wherever possible.

    4.5.2.3.3 Carriers may be limited in their transportation capacity, thus the Planner ensures that 
    multiple Trips, if necessary are attached to the Order.

    -- This requirement is satisfied by the CarriersForRoute function. A trip is not deemd possible if the
    depot for a given carrier in a given city does not have capacity to fulfill it. Thus the Planner does
    not have to intentionally check if availability is present; the OSHT TMS system performs this automatically.

    4.5.2.3.4 The Planner may simulate the passage of time in 1-day increments in order to mover 
    Orders and their trips to completed state

    -- KIND OF SATISFIED? I have a simple little Console loop that cycles through each Trip
    with some Thread.Sleep calls for fun. Really just a gimmick...

    4.5.2.3.5 Planner may confirm an order is completed. Completed Orders are marked for follow-up 
    from the Buyer

    -- Satisfied; IsComplete db flag is set, Buyer can select from those db entries where IsComplete=true
    to generate an invoice from them.

    4.5.2.3.6 Planner may see a summary of all active Orders in a status screen.

    -- Satisfied to my best understanding; all unfinished orders can be retrieved from the TMS database and
    given to the user.

4.5.2.3.7 (Optional) The status screen is highly graphical, perhaps using some kind of Google Maps 
plugin.

-- Sorry Laura, but it's the 4th, I'm busy on the 7th, so I need this done for 2pm of the 7th at the
latest. No chance here. Maybe if my group members had done anything at all at this point, we'd be in a
different position.

4.5.2.3.8 Planner may generate a summary report of all Invoice data for a) all time, and b) The 
‘past 2 weeks’ of simulated time.

-- UNFINISHED
 */


using System;
using SENG2020_TermProject.Communications;
using SENG2020_TermProject.Data_Logic;
using System.Collections.Generic;

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
            if (orders.Length == 0)
                Console.WriteLine("No Orders Available!\n");

        }

        public List<Trip> GetRoute(Order o)
        {
            List<List<Trip>> routes = CarrierList.CarriersForOrder(o);
            if (routes == null)
            {
                FileAccess.Log(String.Format("ERROR - No logical routes available given origin city and destination.\n{0} --> {1}", o.GetOrigin(), o.GetDestin()));
                Console.WriteLine("No Logical Routes!");
            }
            else
            {
                int RouteIterator = 0;
                foreach (List<Trip> l in routes)
                {
                    Console.WriteLine("Route #{0}", RouteIterator);
                    Console.WriteLine("==========");
                    int TripIterator = 0;
                    foreach (Trip t in l)
                    {
                        Console.WriteLine("\tTrip #{0}", TripIterator);
                        Console.WriteLine("\t\tFrom {0}\t-->\t{1}", t.GetOrigin().GetName(), t.GetDestin().GetName());
                        Console.WriteLine("\t\tDistance:\t{0}km", t.GetTripDistance());
                        Console.WriteLine("\t\tTime:\t\t{0}h", t.GetTripTime(o.mr.GetJobType()));
                        Console.WriteLine("\t\tCost:\t\t${0}", t.GetTripCost(o.mr.GetJobType(), o.mr.GetQuantity()));
                        Console.WriteLine("\t\tCarrier:\t{0}", t.GetCarrier().GetName());

                        Console.WriteLine();

                        TripIterator++;
                    }
                    RouteIterator++;
                }

                Console.WriteLine("Select a route to fulfill this order:");
                int inp = GetIntBetween(routes.Count - 1, 0);
                return routes[inp];
            }

            return null;
        }

        private void GetDatabaseAccess()
        {
            if (this.tms != null) return;

            Console.WriteLine("Enter the Planner TMS Database password: ");
            tms = new DatabaseManagement.TMSDatabaseAccess("planner", GetInput());
        }

        //renns
        public void PlannerWorkFlow() //this will actually take a value from a database based on user input ! :)
        {
            PlannerHeader();
            GetDatabaseAccess();
            Console.WriteLine("\n\n");
            if (!this.tms.ValidConnection)
            {
                Console.WriteLine("Invalid TMS Database username or password!");
                FileAccess.Log("Invalid Login Attempt by user Planner");
                return;
            }// exit the application immediately upon establishing the connection is invalid

            Order[] orders;
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("1. View Unfilled Orders");
                Console.WriteLine("2. Prepare and Simualte an Order");
                Console.WriteLine("3. Generate Invoice Summary");
                Console.WriteLine("0. Exit");
                inp = GetInput();

                if (inp == "1")
                {
                    Delay();
                    DisplayUnfilledOrders();
                }
                else if (inp == "2")
                {
                    Delay();
                    DisplayUnfilledOrders();
                    orders = tms.GetIncompleteOrders();

                    if (!(orders.Length == 0))
                    {
                        Console.WriteLine("Select an order to fulfill - (0 - {0}).", orders.Length - 1);
                        Order o = orders[GetIntBetween(orders.Length - 1, 0)];

                        //nested a GetRoute call with o into o's prep order func.
                        o.PrepOrder(GetRoute(o));
                        Delay();
                        o.Display();

                        Console.WriteLine("Set this Order to finished state? Y/N");
                        inp = GetYesNo();

                        if(inp == "Y")
                        {
                            Delay();
                            o.SimulateTime();
                            if (tms.SetOrderComplete(o))
                            {
                                Console.WriteLine("Sucessfully finished Order #{0} - {1}", o.GetID(), o.mr.GetClientName());
                            }
                            else
                            {
                                Console.WriteLine("An error occured in TMSDatabaseAccess.cs - See Logs");
                            }
                        }
                    }
                }
                else if(inp == "3")
                {
                    //FileAccess class to iterate through invoices here
                }
                else if (inp == "0")
                {
                    inp = null;
                }
            }
        }
    }
}
