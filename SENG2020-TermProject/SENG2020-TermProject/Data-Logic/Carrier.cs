/*
 * FILE             : Carrier.cs
 * PROJECT          : SENG2020 Term Project
 * PROGRAMMER       : Ryan Enns
 * FIRST VERSION    : 2021-11-26
 * DESCRIPTION      :
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG2020_TermProject.Data_Logic
{
    /**
     * \brief       Encapsulates all data regarding a Carrier.
     * 
     * \details     The Carrier class represents one of the four potential carriers
     *              available to the TMS system. It uses a String to represent the
     *              name of the carrier and a List of object type Depot to represent
     *              each of the potential multiple depots that the carrier might have.
     */
    class Carrier
    {
        /// \brief      The name of the carrier.
        public String CarrierName;
        /// \brief      The list of depots this carrier has.
        public List<Depot> Depots;

        /// \brief      Sets the carrier name to paramater cn; copies the List<Depot> parameter to the local Depots list.
        public Carrier(String cn, List<Depot> d)
        {
            CarrierName = cn;
            Depots = d;
        }

        /// \brief      Dumps all data about this respective carrier to the console.
        public void Display()
        {
            Console.WriteLine("==============\n{0}\n==============", CarrierName);
            Console.WriteLine("Total Depots:\t\t{0}\n", this.Depots.Count);
            foreach (Depot d in Depots)
                d.Display();
        }
    }

    /**
     * \brief       Represents a depot that can be had by a carrier.
     * 
     * \details     Carriers are companies that have depots in different cities that can be
     *              shipped to and from. The Depot class exists solely to serve the Carrier
     *              class to contain information about each of that respective carrier's
     *              depots. The depot class contains fields for the name of the city it's in,
     *              the availability of FTL and LTL, FTL and LTL rates, and the reefer charge.
     */
    class Depot
    {
        /// \brief      The name of the city that the depot is located in.
        public String CityName;
        /// \brief      The number of available full truck load orders this depot can take.
        public int FTLAvail;
        /// \brief      The number of pallettes on ltl orders this depot can take.
        public int LTLAvail;
        /// \brief      The per km charge for a full truck load order.
        public double FTLRate;
        /// \brief      The per km charge for a single pallette for ltl orders.
        public double LTLRate;
        /// \brief      The percentage increase in rate when a reefer is used instead of a dry van.
        public double reefCharge;

        /// \brief      
        public Depot(String cn, int fa, int la, double fr, double lr, double rc)
        {
            CityName = cn;
            FTLAvail = fa;
            LTLAvail = la;
            FTLRate = fr;
            LTLRate = lr;
            reefCharge = rc;
        }

        /// \brief      Displays all info about the respective Depot.
        public void Display()
        {
            Console.WriteLine("Depot City:\t\t{0}", this.CityName);
            Console.WriteLine("FTL Availability:\t{0}", this.FTLAvail);
            Console.WriteLine("LTL Availability:\t{0}", this.LTLAvail);
            Console.WriteLine("FTL Rate:\t\t{0}", this.FTLRate);
            Console.WriteLine("LTL Rate:\t\t{0}", this.LTLRate);
            Console.WriteLine("Reefer Charge:\t\t{0}", this.reefCharge);
            Console.WriteLine();
        }
    }

    /**
     * \brief
     * 
     * \details
     */
    static class CarrierList
    {
        /// \brief      The list of Carriers that can be selected for shipments.
        private static List<Carrier> Carriers = new List<Carrier>();

        /// \brief      Populates the Carriers List<Carrier> with the default
        ///             carrier values.
        static CarrierList()
        {
            //Planet Express
            List<Depot> pe = new List<Depot>();
            pe.Add(new Depot("Windsor", 50, 640, 5.21, 0.3621, 0.08));
            pe.Add(new Depot("Hamilton", 50, 640, 5.21, 0.3621, 0.08));
            pe.Add(new Depot("Oshawa", 50, 640, 5.21, 0.3621, 0.08));
            pe.Add(new Depot("Belleville", 50, 640, 5.21, 0.3621, 0.08));
            pe.Add(new Depot("Ottawa", 50, 640, 5.21, 0.3621, 0.08));

            Carriers.Add(new Carrier("Planet Express", pe));

            //Schooner's
            List<Depot> sc = new List<Depot>();
            sc.Add(new Depot("London", 18, 98, 5.05, 0.3434, 0.07));
            sc.Add(new Depot("Toronto", 18, 98, 5.05, 0.3434, 0.07));
            sc.Add(new Depot("Kingston", 18, 98, 5.05, 0.3434, 0.07));

            Carriers.Add(new Carrier("Schooner's", sc));

            //Tillman Transport
            List<Depot> tt = new List<Depot>();
            tt.Add(new Depot("Windsor", 24, 35, 5.11, 0.3012, 0.09));
            tt.Add(new Depot("London", 18, 45, 5.11, 0.3012, 0.09));
            tt.Add(new Depot("Hamilton", 18, 45, 5.11, 0.3012, 0.09));

            Carriers.Add(new Carrier("Tillman Transport", tt));

            //We Haul
            List<Depot> wh = new List<Depot>();

            wh.Add(new Depot("Ottawa", 11, 0, 5.2, 0, 0.065));
            wh.Add(new Depot("Toronto", 11, 0, 5.2, 0, 0.065));

            Carriers.Add(new Carrier("We Haul", wh));
        }

        public static void Display()
        {
            foreach (Carrier c in Carriers)
                c.Display();
        }
    }
}
