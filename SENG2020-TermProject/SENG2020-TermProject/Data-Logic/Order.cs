/*
* FILE             : Order.cs
* PROJECT          : SENG2020 - Term Project
* PROGRAMMER(s)    : Ryan Enns
* FIRST VERSION    : 2021-11-25
* DESCRIPTION      :
*   This is the Order object. The Order object is used to represent exactly that; an Order.
*   
*   The Order contains multiple fields to store various information about the given Order instance.
*   First, it includes a MarketplaceRequest, to store the MarketplaceRequest data it was created from.
*   It may make more sense to create separate members to store this data, however from a memory and
*   efficiency standpoint, copying the MarketplaceRequest object makes life easier for everyone.
*   
*   Most of the Order's own members are only populated once the Order has been "prepared". What occures
*   what an Order is prepared? Great question. An Order is prepared by the Planner when they select
*   Carriers to satisfy an Order. Upon selection, the Order's fields can be calculated using the
*   Carrier and Depot information given to it by the Planner's Carrier selection.
*   
*   The Order class contains all functions needed to calculate its own distance, cost, elapsed time,
*   and so forth, given that it has a populated trips member to work with. It mostly does this
*   with logic found in the Trip class.
*/

using System.Collections.Generic;
using System;

//https://conestoga.desire2learn.com/d2l/le/content/482677/viewContent/9959524/View

namespace SENG2020_TermProject.Data_Logic
{
    /**
     * \brief       The Order class represents an order that can be created by a Buyer's request.
     * 
     * \details     Orders are created by Buyers. Orders are comprised of a Marketplace Request,
     *              a singular carrier or group of carriers who are available to fulfill this
     *              request, and finally an invoice that will be stored in the TMS database and
     *              be served to the customer.
     */
    class Order
    {
        
        private int ID = -1; //-1 is default for unprepped order
        public bool isprepped = false; //bless :"D

        /**
         * \brief       mr represents the MarketplaceRequest object that this Order was created from.
         * 
         * \details     As specified in the technical requirements of this software, Orders are created by
         *              Buyers. The Buyer requests contracts from the Contract Marketplace - the back end
         *              of which is specified under Communications/DatabaseManagement/ContractMarketAccess.
         *              The buyer can then process this contract into an order. Thus the cornerstone of the
         *              order is a MarketplaceRequest. The Order class has a private MarketplaceRequest
         *              member to represent the MarketplaceRequest from which the Order was derived.
         */
        public MarketplaceRequest mr = null;

        /// \brief      A boolean that indicates if this order has been fulfilled or not.
        private bool IsComplete;
        /// \brief      A float that represents how many hours the selected shipping sequence will take
        private double TimeToComplete;
        /// \brief      The total distance between the start city and end city.
        private int DistanceToComplete;
        /// \brief      The total cost of the shipment using the given carrier.
        private double CostToComplete = 0.0;
        /// \brief      The surcharge that OSHT will apply for profit to this order.
        private double OSHTSurcharge = 0.0;
        /// \brief      The carrier being used to ship this order.
        //private Carrier c = null;
        private List<Trip> trips = new List<Trip>();

        /**
         * \brief       Creates an order based on the information given to us by a MarketplaceRequest.
         * 
         * \details     This constructor accepts a MarketplaceRequest object to then pass to the ParseMarketplaceRequest
         *              function for further processing.
         */
        public Order(MarketplaceRequest req) : this()
        {
            this.mr = req;
        }

        public Order(int nid, MarketplaceRequest req) : this()
        {
            this.mr = req;
            this.ID = nid;
        }

        public Order(int nid, MarketplaceRequest req, double t, int d, double cost, double osh, bool c)
        {
            this.ID = nid;
            this.mr = req;
            this.IsComplete = c;
            this.TimeToComplete = t;
            this.DistanceToComplete = d;
            this.CostToComplete = cost;
            this.OSHTSurcharge = osh;
            this.isprepped = true;
        }


        public int GetID()
        {
            return this.ID;
        }
        /*
        public String GetCarrierName()
        {
            return this.c.CarrierName;
        }
        */
        public String GetOrigin()
        {
            return this.mr.GetCityOrigin();
        }

        public String GetDestin()
        {
            return this.mr.GetCityDestination();
        }

        public String GetClient()
        {
            return this.mr.GetClientName();
        }

        public double GetTimeToComplete()
        {
            return this.TimeToComplete;
        }

        public double GetDistanceToComplete()
        {
            return this.DistanceToComplete;
        }

        public double GetCostToComplete()
        {
            return this.CostToComplete;
        }

        public double GetOSHTSurcharge()
        {
            return this.OSHTSurcharge;
        }

        public bool GetIsComplete()
        {
            return this.IsComplete;
        }

        public Order()
        {
            IsComplete = false;
            TimeToComplete = -1.0;
            DistanceToComplete = 0;
        }

        public String JobType()
        {
            if (mr.GetJobType() == 0)
                return "FTL";
            else
                return "LTL";
        }

        public int LTLQty()
        {
            if (this.JobType() == "LTL")
                return this.mr.GetQuantity();
            else return -1;
        }

        /**
         * \brief       Populates Order instance fields based on information supplied by the MarketplaceRequest parmaeter.
         * 
         * \details     This function accepts a MarketplaceRequest object to create an order from. First, the req paramater
         *              is copied to the local MarketplaceRequest instance (mr). Then, the CalculateDistance and CalculateTime
         *              functions are called to further populate the Order fields.
         */
        /*
         * NAME : PrepOrder
         * DESC :
         *  This function "prepares" an order for fulfillment. This means that the carriers it needs to use are established,
         *  and from this the distance, time and cost are established using various other member functions.
         * RTRN : bool
         * PARM : //
         */
        public bool PrepOrder(List<Trip> route)
        {
            if (isprepped) throw new Exception("Order already prepared.");
            if(this.mr != null)
            {
                this.trips = route;
                if(this.trips == null)
                    return false;
                CalculateDistance();
                CalculateTime();
                CalculateCost();

                isprepped = true;
            }
            return this.isprepped;
        }

        /**
         * \brief       Assigns a value to the DistanceToComplete member.
         * 
         * \details     This function calls the CityList.DrivingDistance with its mr.CityOrigin and mr.CityDestin fields as
         *              parameters. If mr is null, these function calls are not made.
         */
        private void CalculateDistance()
        {
            if(this.mr!=null)
                DistanceToComplete += CityList.DrivingDistance(mr.GetCityOrigin(), mr.GetCityDestination());
        }

        /**
         * \brief       Assigns a value to the TimeToComplete member.
         * 
         * \details     This function, assuming mr is not null, assigns value to the TimeToComplete member. Frist, the
         *              CityList.DrivingTime function is called with mr.CityOrigin and mr.CityDestin as parameters; then,
         *              2 * LOAD_UNLOAD_TIME is added. Finally, if the job type is LTL, we add 2 for each stop that occurs
         *              between the cities, as defined by the CityList.LTLStops function with the same parameters.
         */
        /*
         * NAME : CalculateTime
         * DESC :
         * The CalculateTime function calculates the total time needed to complete an order
         * by iterating through all Trip instances in its trips list. It adds all of these
         * up in the TimeToComplete member.
         * RTRN : //
         * PARM : //
         */
        private void CalculateTime()
        {
            this.TimeToComplete = 0.0;
            foreach(Trip t in trips)
            {
                this.TimeToComplete += t.GetTripTime(this.mr.GetJobType());
            }
        }

        /*
         * NAME : CalculateCost
         * DESC :
         *  The CalculateCost function calculates an order's total cost by iterating and adding up
         *  the results of the Trip.GetTripCost function for each individual trip in the respective
         *  Order instance. Then, based on the job type, we apply the 8% or 5% surcharge to populate
         *  the OSHTSurcharge method. Finally, the TotalCost member that is being used to track the
         *  total Carrier charges of the order is copied to the CostToComplete member.
         *  
         */
        private void CalculateCost()
        {
            //The carrier object we take here will dictate the cost per km
            //this Order instance NEEDS to be init for this func to work

            if (this == null) return;

            double TotalCost = 0.0;

            foreach (Trip t in trips)
            {
                TotalCost += t.GetTripCost(this.mr.GetJobType(), this.mr.GetQuantity(), this.mr.GetVanType());
            }

            if (this.JobType() == "FTL")
                this.OSHTSurcharge = TotalCost * 0.08;
            else this.OSHTSurcharge = TotalCost * 0.05;

            this.CostToComplete = TotalCost;
        }

        /*
         * NAME : Dispaly
         * DESC :
         *  The Display function neatly organizes and displays information about
         *  an order to the console. Varying on trip length and ID number, some fields
         *  are included or discluded.
         */
        public void Display()
        {
            if (!(this.ID == -1))
                Console.WriteLine("Database ID:\t\t#{0}", this.ID);
            this.mr.Display();
            if (this.isprepped == true)
            {
                Console.WriteLine("Time to complete:\t{0}h", this.TimeToComplete);
                Console.WriteLine("Distance to destin:\t{0}km", this.DistanceToComplete);
                Console.WriteLine("Carrier Charges:\t${0}", this.CostToComplete);
                Console.WriteLine("OSHT Charges:\t\t${0}", this.OSHTSurcharge);
                Console.WriteLine("Total cost to complete:\t${0}", this.CostToComplete + this.OSHTSurcharge);
                Console.WriteLine();
                
                if(!(trips == null || trips.Count == 0))
                    Console.WriteLine("Trips to Compelte:\t{0}", this.trips.Count);

                foreach (Trip t in trips)
                {
                    Console.WriteLine("================================");
                    Console.WriteLine("Trip Origin:\t\t{0}", t.GetOrigin().GetName());
                    Console.WriteLine("Trip Destination:\t{0}", t.GetDestin().GetName());
                    Console.WriteLine("Carrier to Complete:\t{0}", t.GetCarrier().GetName());
                    Console.WriteLine("Trip Distance:\t\t{0}km", t.GetTripDistance());
                    Console.WriteLine("Trip Cost:\t\t${0}", t.GetTripCost(this.mr.GetJobType(), this.mr.GetQuantity()));
                    Console.WriteLine("Trip Time:\t\t{0}h", t.GetTripTime(this.mr.GetJobType()));
                    Console.WriteLine();
                }
            }
        }

        public void SimulateTime()
        {
            if (!this.isprepped) return;

            Console.WriteLine("Simulating Order #{0}", this.ID);
            int TripIter = 1;
            foreach (Trip t in this.trips)
            {
                double hoursPassed = 0.0;
                Console.WriteLine("Trip #{0} / {1}", TripIter, this.trips.Count);
                for(hoursPassed = 0.0; hoursPassed < t.GetTripTime(this.mr.GetJobType()); hoursPassed++)
                {
                    System.Threading.Thread.Sleep(750);
                    Console.WriteLine("{0}h / {1}h", hoursPassed, t.GetTripTime(this.mr.GetJobType()));
                }
                System.Threading.Thread.Sleep(750);
                hoursPassed = t.GetTripTime(this.mr.GetJobType());
                Console.WriteLine("{0}h / {1}h", hoursPassed, t.GetTripTime(this.mr.GetJobType()));
                System.Threading.Thread.Sleep(750);
                Console.WriteLine("Trip #{0} complete!", TripIter);
                TripIter++;
            }
        }
    }
}
