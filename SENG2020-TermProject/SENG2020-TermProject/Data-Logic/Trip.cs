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
            Time += CityList.DrivingTime(Origin.CityName, Destin.CityName);

            if (JobType == 1)
            {
                Time += 2 * CityList.LTLStops(this.Origin.CityName, this.Destin.CityName);
            }

            return Time;
        }
    }
}
