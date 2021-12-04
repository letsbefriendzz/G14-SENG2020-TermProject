/*
* FILE             : Order.cs
* PROJECT          : SENG2020 - Term Project
* PROGRAMMER(s)    : Ryan Enns
* FIRST VERSION    : 2021-12-03
* DESCRIPTION      :
*   The Trip class was an embarassingly late yet critical addition to this back end.
*   Why is it so critical? Well, how else are you gonna store multi city routes if
*   your Object class only has storage options for an origin and destination city?
*   
*   Enter: The Trip Class.
*   
*   The Trip class defines an origin and destination city, just like an Order does.
*   Thus the Trip class is only extra overhead for single carrier solutions. However,
*   this quickly becomes rectified by the fact that for double carrier solutions, which
*   do in fact comprise a surprising amount of routes, the Trip class is absolutely
*   amazing.
*   
*   The Trip class has only three fields. Two City objects for its Origin and Destination,
*   and a Carrier instance for the Carrier that can perform this trip. A Carrier that can
*   perform the trip is defined as a Carrier that has depots in the Origin and Destination
*   cities.
*   
*   The Trip class contains three functions to determine the cost of the trip, the distance
*   travelled during the trip, and the total time that will elapse during a trip. Sadly,
*   the GetTripCost function does rely on Order information being passed in the form of
*   the JobType and quantity. I may replace this with a GetFTLCost and GetLTLCost later
*   for good design reasons, however I have very little time on my hands and what works
*   right now, works. So, why reinvent the wheel?
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG2020_TermProject.Data_Logic
{
    class Trip
    {
        /// \brief      This macro defines the time requried to load and unload at the origin and destination.
        ///             baically, just add this twice to the total time.
        private const int LOAD_UNLOAD_TIME = 2;

        private City Origin;
        private City Destin;
        private Carrier Carr;

        public Trip(City o, City d, Carrier c)
        {
            this.Origin = o;
            this.Destin = d;
            this.Carr = c;
        }

        public Trip(String o, String d, Carrier c)
        {
            this.Origin = CityList.GetCity(o);
            this.Destin = CityList.GetCity(d);
            this.Carr = c;
        }

        public City GetOrigin()
        {
            return this.Origin;
        }

        public City GetDestin()
        {
            return this.Destin;
        }

        public Carrier GetCarrier()
        {
            return this.Carr;
        }

        /*
         * NAME : GetTripCost
         * DESC :
         *  This function, using the job type and quantity values provided, determines the
         *  cost of this specific trip from Origin to Destin. It gets the respective FTL
         *  and LTL rates from the GetDepot function that's available in the local Carrier
         *  instance. Appropriate math is applied (distance * rate) & (distance * rate * pallets)
         *  for FTL and LTL, and the established cost is returned.
         *  
         *  TODO
         *  - REEFER CHARGE
         *  - CHECK FOR LTL / FTL AVAIL
         * RTRN : double
         * PARM ; int, int
         */
        public double GetTripCost(int type, int quantity, int reefer = 0)
        {
            double cost = 0.0;
            if(type == 0)
            {
                double FTLRate = Carr.GetDepot(Origin.GetName()).FTLRate;
                cost = GetTripDistance() * FTLRate;
            }
            else
            {
                // LTL rates from depots are on a per pallet per km basis; this means that we need
                // to get the LTLRate of the origin city depot and multiple it by the quantity.
                double LTLRate = Carr.GetDepot(Origin.GetName()).LTLRate * quantity;
                cost = GetTripDistance() * LTLRate;
            }

            if (reefer == 1)
                cost += cost * Carr.GetDepot(Origin.GetName()).reefCharge;

            return cost;
        }

        /*
         * NAME : GetDistance()
         * DESC :
         *  The GetDistance function uses the CityList.DrivingDistance function to determine
         *  the distance between this trip's origin and destination city.
         * RTRN : int
         * PARM : //
         */
        public int GetTripDistance()
        {
            return CityList.DrivingDistance(Origin.GetName(), Destin.GetName());
        }

        /*
         * NAME : GetTripTime
         * DESC :
         *  The GetTripTime() function determines the amount of time that it takes to fulfill a given
         *  trip. Some explanation in the methodology used to established this number is needed to fully
         *  understand.
         *  
         *  note: this function supplies the time needed for a single trip; the Order class iterates over
         *  this function for each trip it needs to complete and combines the results of this function into
         *  the values that can be found in Order object instances.
         *  
         *  CALCULATING FTL TIMES
         *  
         *  FTL times are easy - 2 hours for load, 2 hours for unload, the constant time between cities as
         *  defined in the CityList.
         *  
         *  CALCULATING LTL TIMES
         *  
         *  LTL times are calculated as the previous FTL function, however 2h is added for each stop that
         *  must occur between cities. This can be thought of as Time += 2 * CityList.LTLStops(origin, destin).
         *  
         * RTRN : double
         * PARM : JobType
         */
        public double GetTripTime(int JobType)
        {
            double Time = 0.0;
            Time += LOAD_UNLOAD_TIME * 2;
            Time += CityList.DrivingTime(Origin.GetName(), Destin.GetName());

            if (JobType == 1)
            {
                Time += 2 * CityList.LTLStops(this.Origin.GetName(), this.Destin.GetName());
            }

            return Time;
        }
    }
}
