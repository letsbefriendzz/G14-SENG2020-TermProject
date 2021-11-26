using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG2020_TermProject.Data_Logic
{
    class Carrier
    {
        public String CarrierName;
        public List<Depot> Depots;
    }

    class Depot
    {
        public String CityName;
        public int FTLAvail;
        public int LTLAvail;
        public double FTLRate;
        public double LTLRate;
        public double reefCharge;
    }

    static class CarrierList
    {
    }
}
