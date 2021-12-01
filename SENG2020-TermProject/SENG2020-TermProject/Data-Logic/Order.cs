/*
* FILE             : Order.cs
* PROJECT          : SENG2020 - Term Project
* PROGRAMMER(s)    : Ryan Enns
* FIRST VERSION    : 2021-11-25
* DESCRIPTION      :
*/

using System.Collections.Generic;
using System;

//reference this fucker like crazy!
//https://conestoga.desire2learn.com/d2l/le/content/482677/viewContent/9959524/View

/*
    
GOALS FOR THIS CLASS

The order class needs to represent an order. Shocker, thanks Ryan, you're a genius. What does that mean?
An order is first initialized by a Buyer, who finds the contract on the marketplace and creates an order
to fill it.

*/

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
        /// \brief      This macro defines the time requried to load and unload at the origin and destination.
        ///             baically, just add this twice to the total time.
        const int LOAD_UNLOAD_TIME = 2;
        public bool fulfilled = false;

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
        private MarketplaceRequest mr = null;

        /// \brief      A boolean that indicates if this order has been fulfilled or not.
        public bool IsComplete;
        /// \brief      A float that represents how many hours the selected shipping sequence will take
        public double TimeToComplete;
        /// \brief      The total distance between the start city and end city.
        public int DistanceToComplete;
        /// \brief      The total cost of the shipment using the given carrier.
        public double CostToComplete = -1.0; //default -1, unset flag
        /// \brief      The carrier being used to ship this order.
        private Carrier c = null;

        public String GetOrigin()
        {
            return this.mr.CityOrigin;
        }

        public String GetDestin()
        {
            return this.mr.CityDestin;
        }

        public Order()
        {
            IsComplete = false;
            TimeToComplete = 0.0;
            DistanceToComplete = 0;
        }

        /**
         * \brief       Creates an order based on the information given to us by a MarketplaceRequest.
         * 
         * \details     This constructor accepts a MarketplaceRequest object to then pass to the ParseMarketplaceRequest
         *              function for further processing.
         */
        public Order(MarketplaceRequest req)
        {
            ParseMarketplaceRequest(req);
        }

        /**
         * \brief       Populates Order instance fields based on information supplied by the MarketplaceRequest parmaeter.
         * 
         * \details     This function accepts a MarketplaceRequest object to create an order from. First, the req paramater
         *              is copied to the local MarketplaceRequest instance (mr). Then, the CalculateDistance and CalculateTime
         *              functions are called to further populate the Order fields.
         */
        private void ParseMarketplaceRequest(MarketplaceRequest req)
        {
            if(req != null)
            {
                this.mr = req;
                CalculateDistance();
                CalculateTime();
            }
        }

        private void GetCarrier()
        {
            this.c = CarrierList.CarriersForRoute(this);
            if(this.c != null)
            {
                this.CalculateCost(c);
            }
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
                DistanceToComplete += CityList.DrivingDistance(mr.CityOrigin, mr.CityDestin);
        }

        /**
         * \brief       Assigns a value to the TimeToComplete member.
         * 
         * \details     This function, assuming mr is not null, assigns value to the TimeToComplete member. Frist, the
         *              CityList.DrivingTime function is called with mr.CityOrigin and mr.CityDestin as parameters; then,
         *              2 * LOAD_UNLOAD_TIME is added. Finally, if the job type is LTL, we add 2 for each stop that occurs
         *              between the cities, as defined by the CityList.LTLStops function with the same parameters.
         */
        private void CalculateTime()
        {
            if (mr == null) return;

            this.TimeToComplete += CityList.DrivingTime(mr.CityOrigin, mr.CityDestin);
            this.TimeToComplete += (LOAD_UNLOAD_TIME * 2);
            if (mr.JobType == 1)
            {
                for(int i = 0; i < CityList.LTLStops(mr.CityOrigin, mr.CityDestin); i++)
                {
                    TimeToComplete += 2;
                }
            }
        }

        private void CalculateCost(Carrier c)
        {

        }

        public void Display()
        {
            this.mr.Display();
            Console.WriteLine("Time to complete:\t{0}h", this.TimeToComplete);
            Console.WriteLine("Distance to destin:\t{0}km", this.DistanceToComplete);
        }
    }
}
