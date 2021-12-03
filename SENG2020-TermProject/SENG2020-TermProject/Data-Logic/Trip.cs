using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG2020_TermProject.Data_Logic
{
    class Trip
    {
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

        public double GetTripCost(int type, int quantity)
        {
            double cost = 0.0;
            double FTLRate;
            if(type == 0)
            {
                FTLRate = Carr.GetDepot(Origin.GetName()).FTLRate;
                cost = GetDistance() * FTLRate;
            }
            else
            {

            }

            return cost;
        }

        public int GetDistance()
        {
            return CityList.DrivingDistance(Origin.GetName(), Destin.GetName());
        }
    }
}
