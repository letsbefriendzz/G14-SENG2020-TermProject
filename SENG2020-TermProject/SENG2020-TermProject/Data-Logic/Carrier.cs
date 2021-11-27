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

        /**
         * \brief       Checks to see if a Carrier has a depot in a given city.
         * 
         * \details     This function iterates through the respective Carrier instance's List<Depot>, comparing
         *              the Depot.CityName with the String cn passed as a parameter. If it finds a match, true is
         *              returned. Otherwise, false is returned.
         * 
         * \retval      Returns a boolean; true or false.
         */
        public bool HasDepotIn(String cn)
        {
            foreach(Depot d in Depots)
                if (d.CityName == cn)
                    return true;
            return false;
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

        /**
         * \brief       Checks if a depot has full truck loads available.
         * 
         * \details     Compares the FTL available of the given Depot with the FTL needed integer passed
         *              as a parameter. If the FTL available exceeds the FTL needed, true is returned.
         *              Otherwise, false is returned.
         * 
         * \retval      Returns a boolean; true or false.
         */
        public bool HasFTLAvail(int FTLNeeded)
        {
            if (this.FTLAvail > FTLNeeded)
                return true;
            return false;
        }

        /**
         * \brief       Checks if a depot has limited truck loads available.
         * 
         * \details     Compares the LTL available of the given Depot with the LTL needed integer passed
         *              as a parameter. If the LTL available exceeds the LTL needed, true is returned.
         *              Otherwise, false is returned.
         * 
         * \retval      Returns a boolean; true or false.
         */
        public bool HasLTLAvail(int LTLneeded)
        {
            if (this.LTLAvail > LTLneeded)
                return true;
            return false;
        }
    }

    /**
     * \brief       A static list of available carriers for shipments.
     * 
     * \details     The CarrierList class contains a single List of type Carrier. This class is used
     *              to access the available carriers to calculate shipping routes.
     */
    static class CarrierList
    {
        /// \brief      The list of Carriers that can be selected for shipments.
        private static List<Carrier> Carriers = new List<Carrier>();

        /** \brief      Populates the Carriers List<Carrier> with the default
         *              carrier values.
         *              
         * \details     This function will eventually access the Carrier update system to get live updates
         *              about availabilty. Until this has been implemented (see Communications folder for
         *              progress updates), we will use these hardcoded default values for each instance.
         */
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

        /**
         * \brief       Establishes which carrier will be used for an order.
         * 
         * \details     This function accepts an Order parameter and compares the start and end destinatino
         *              with the various available carriers that OSHT has contact with. It returns the Carrier
         *              instance that has depots in both the origin and destination cities for the given order.
         *              If two carriers can perform the same delivery, a cost effectiveness algorithm is deployed
         *              and the cheapest option available is the one that is chosen.
         * 
         * \returns     An instance of Carrier; the carrier that will be used for the given order.
         */
        public static Carrier CarriersForRoute(Order o)
        {
            if (o == null) return null;

            List<Carrier> l = new List<Carrier>();
            foreach(Carrier c in Carriers)
            {
                if (c.HasDepotIn(o.GetOrigin()) && c.HasDepotIn(o.GetDestin()))
                {
                    l.Add(c);
                }
            }

            //if more than one carrier can fulfill our order...
            if (l.Count > 1)
            {
                //call some cost calculating algorithm here to determine what is best!
                return l[0];
            }
            else if(l.Count != 0)
            {
                return l[0];
            }
            
            return null;
        }
    }
}
