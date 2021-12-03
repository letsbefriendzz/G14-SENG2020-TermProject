/*
 * FILE             : Carrier.cs
 * PROJECT          : SENG2020 Term Project
 * PROGRAMMER       : Ryan Enns - lol don't even put your names on shit I wrote
 * FIRST VERSION    : 2021-11-26
 * DESCRIPTION      :
 */

using System;
using System.Collections.Generic;

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

        public String GetName()
        {
            return this.CarrierName;
        }

        /// \brief      Sets the carrier name to paramater cn; copies the List<Depot> parameter to the local Depots list.
        public Carrier(String cn, List<Depot> d)
        {
            CarrierName = cn;
            Depots = d;
        }

        /// \brief      Dumps all data about this respective carrier to the console.
        public void Display()
        {
            Console.WriteLine("=================");
            Console.WriteLine(CarrierName);
            Console.WriteLine("Total Depots:\t\t{0}\n", this.Depots.Count);
            Console.WriteLine("Has Depots In:");
            foreach (Depot d in Depots)
                Console.WriteLine(" -\t{0}", d.CityName);
            Console.WriteLine("=================");
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

        //fetches a Depot object from a Carrier object
        public Depot GetDepot(String dcn)
        {
            if(HasDepotIn(dcn))
            {
                foreach (Depot d in Depots)
                    if (d.CityName == dcn) return d;
            }
            return null;
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

        public double FTLCharge(int distance)
        {
            return this.FTLRate * distance; //return the distance multiplied by FTLRate
        }

        public double LTLCharge(int distance)
        {
            return this.LTLRate * distance;
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
        /*
         * NAME :
         * DESC :
         * PARM :
         * RTRN :
         */
        public static void Display()
        {
            foreach (Carrier c in Carriers)
                c.Display();
        }

        /*
         * NAME
         * DESC :
            This function has three nested loops. For each city that rests between our origin and destination,
            we iterate through each carrier twice to test if any pair of carriers has a depot in a city between
            the origin and destination. If it does, we add new trips to the trips list and return it. Otherwise,
            we return null.
         * RTRN
         * PARM
         */
        public static List<Trip> FindIntermediaryCity(City c1, City c2)
        {
            List<Trip> trips = new List<Trip>();
            String origin = c1.CityName;
            String destin = c2.CityName;
            int i1 = CityList.GetCityIndex(origin) +1;
            int i2 = CityList.GetCityIndex(destin);

            if(i1 > i2)
            {
                int inter = i1;
                i1 = i2;
                i2 = inter;
            }

            if (i1 == i2)
                Console.WriteLine("No Intermediary Cities");

            for(int i = i1; i < i2; i++)
            {
                String CurrentCity = CityList.CityAt(i).CityName;

                foreach(Carrier carrier1 in Carriers)
                {
                    foreach(Carrier carrier2 in Carriers)
                    {
                        //if the first carrier has a depot in the origin city and the second carrier has a depot in the destination city...
                        if (carrier1.HasDepotIn(origin) && carrier2.HasDepotIn(destin))
                        {
                            //if both carriers have a depot in the intermediary city
                            if(carrier1.HasDepotIn(CurrentCity) && carrier2.HasDepotIn(CurrentCity))
                            {
                                //add a new trip - origin city as origin, intermediary city as destin
                                trips.Add(new Trip(c1, CityList.GetCity(CurrentCity), carrier1));
                                trips.Add(new Trip(CityList.GetCity(CurrentCity), c2, carrier2));

                                return trips;
                            }
                        }
                    }
                }
            }

            return null;
        }

        //testing function
        public static List<Trip> CarriersForRoute(City c1, City c2)
        {
            List<Carrier> l = new List<Carrier>();
            foreach (Carrier c in Carriers)
            {
                //add each potential carrier to the list of potential carriers
                if (c.HasDepotIn(c1.GetName()) && c.HasDepotIn(c2.GetName()))
                {
                    l.Add(c);
                }
            }

            if (l.Count == 0)
            {
                return CarrierList.FindIntermediaryCity(CityList.GetCity(c1.GetName()), CityList.GetCity(c2.GetName()));
            }
            else
            {
                List<Trip> ReturnValue = new List<Trip>();
                ReturnValue.Add(new Trip(c1.GetName(), c2.GetName(), l[0]));
                return ReturnValue;
            }
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
        /*
         * NAME : CarriersForRoute
         * DESC :
         *  The CarriersForRoute function returns a Carrier instance that will be used to fulfill
         *  an Order. If there are multiple Carriers that could satisfy an order, we will run an
         *  algorithm to determine the most cost effective solution.
         *  
         *  This algorithm ignores the potential for orders that have an origin and destination city
         *  that can't be solved by a singular Carrier. This is because, logistically, this would be
         *  a nightmare - and also because coding a solution for that would take up precious time that
         *  I currently just don't have.
         * PARM : Order
         * RTRN : Carrier
         */
        public static List<Trip> CarriersForRoute(Order o)
        {
            if (o == null) return null;

            List<Carrier> l = new List<Carrier>();
            foreach(Carrier c in Carriers)
            {
                //add each potential carrier to the list of potential carriers
                if (c.HasDepotIn(o.GetOrigin()) && c.HasDepotIn(o.GetDestin()))
                {
                    if (o.JobType() == "FTL")
                    {
                        if(c.GetDepot(o.GetOrigin()).HasFTLAvail(1))
                            l.Add(c);
                    }
                    else
                    {
                        if (c.GetDepot(o.GetOrigin()).HasLTLAvail(o.LTLQty()))
                            l.Add(c);
                    }
                }
            }

            if(l.Count == 0)
            {
                return CarrierList.FindIntermediaryCity(CityList.GetCity(o.GetOrigin()), CityList.GetCity(o.GetDestin()));
            }
            else
            {
                List<Trip> ReturnValue = new List<Trip>();
                ReturnValue.Add(new Trip(o.GetOrigin(), o.GetDestin(), l[0]));
                return ReturnValue;
            }
        }

        /*
         * NAME : Contains
         * DESC :
         *  This function returns a Carrier instance if it is present in the local CarrierList.
         * PARM : String
         * RTRN : Carrier
         */
        public static Carrier GetCarrierByName(String cName)
        {
            foreach (Carrier c in CarrierList.Carriers)
                if (c.CarrierName == cName) return c;
            return null;
        }

        //returns a depot from a carrier - if either the depot or the carrier don't
        //exist, null is returned.
        /*
         * NAME : GetCarrierDepot
         * DESC :
         *  This function retrieves a Depot instance from a specified carrier and locaiton.
         *  This allows us to retrieve Depots to establish the costs of shipping to populate
         *  fields in our Order class.
         * PARM : String, String
         * RTRN : Depot
         */
        public static Depot GetCarrierDepot(String CarrierName, String DepotLocation)
        {
            return GetCarrierByName(CarrierName).GetDepot(DepotLocation);
        }
    }
}
