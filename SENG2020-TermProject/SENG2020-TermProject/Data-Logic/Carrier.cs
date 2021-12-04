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

        public Depot GetDepot(City c)
        {
            return this.GetDepot(c.GetName());
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

        public static List<Carrier> GetList()
        {
            return Carriers;
        }

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
         * NAME : OneStopRoutes
         * DESC :
         *  FindIntermediaryCity takes two City objects and establishes a two-trip, two-carrier route
         *  to satisfy the prospective route. How does it do this? I'm glad you asked...
         *  
         *  First, we create some local variables to shorten up the otherwise annoying nested function calls.
         *  I'm deeply regretting referencing City objects mostly by their String name as opposed to the object
         *  themselves. I don't care at this point, but I hope to polish that up a little more later.
         *  
         *  Secondly, as we have to do in other functions that iterate over the CityList, we need to ensure
         *  that our origin and destination are not flipped in List order. Thus if the index of the first city
         *  in the CityList is greater than the index of the second city, we swap those values so we can
         *  iterate through normally.
         *  
         *  Next, if there are no cities between the origin and destination, we pop a message to the console.
         *  This is for debug purposes and will likely be removed for the final product.
         *  
         *  Finally, we start to actually do some comparisons. We start with a for loop that iterates through
         *  the CityList using our origin and destination city indices. We create a String of the current City
         *  name comparison purposes.
         *  
         *  Next, we enter a foreach loop that iterates through each Carrier in the CarrierList. Then, we enter
         *  a second foreach loop to iterate through each Carrier in the CarrierList during each super-loop
         *  CarrierList iteration.
         *  
         *  In this nested set of loops, we check if the first carrier and second carrier we're comparing
         *  have depots in the origin city (in the former) and destination city (in the latter). We also check,
         *  based on the job type, if there is availability in these cities. If there is, we use these carriers.
         *  
         *  If this condition is met, we check if these two carriers share a depot in the current City that is
         *  between our origin and destination. If it does, we add two new trips to the trips list; one from
         *  origin to the intermediary city using the first carrier, and the second trip from the intermediary
         *  city to the destination city using the second carrier.
         *  
         *  Ryan Enns, 2021-12-03
         * RTRN : List<Trip>
         * PARM : City, City
         */
        public static List<Trip> OneStopRoutes(City c1, City c2, int JobType, int quantity = 0)
        {
            List<List<Trip>> TripsList = new List<List<Trip>>();
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
                        if(carrier1.CarrierName != carrier2.CarrierName && CurrentCity != origin && CurrentCity != destin)
                        {
                            //if the first carrier has a depot in the origin city and the second carrier has a depot in the destination city...
                            if (carrier1.HasDepotIn(origin) && carrier2.HasDepotIn(destin))
                            {
                                //if both carriers have a depot in the intermediary city
                                if (carrier1.HasDepotIn(CurrentCity) && carrier2.HasDepotIn(CurrentCity))
                                {
                                    //if origin and intermeidary have FTL or LTL avail
                                    //add a new trip - origin city as origin, intermediary city as destin

                                    bool AddOptionFlag = false;

                                    if (JobType == 0)
                                    {
                                        if (carrier1.GetDepot(c1).FTLAvail > 0 && carrier2.GetDepot(CurrentCity).FTLAvail > 1)
                                        {
                                            AddOptionFlag = true;
                                        }
                                    }
                                    else
                                    {
                                        if (carrier1.GetDepot(c1).LTLAvail > quantity && carrier2.GetDepot(CurrentCity).FTLAvail > 1)
                                        {
                                            AddOptionFlag = true;
                                        }
                                    }

                                    if (AddOptionFlag)
                                    {
                                        List<Trip> t = new List<Trip>();

                                        t.Add(new Trip(c1, CityList.GetCity(CurrentCity), carrier1));
                                        t.Add(new Trip(CityList.GetCity(CurrentCity), c2, carrier2));

                                        TripsList.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*
            int iter = 0;
            foreach(List<Trip> l in TripsList)
            {
                Console.WriteLine("Route #{0}", iter);
                Console.WriteLine("================================");
                foreach (Trip t in l)
                {
                    Console.WriteLine("================================");
                    Console.WriteLine("Trip Origin:\t\t{0}", t.GetOrigin().GetName());
                    Console.WriteLine("Trip Destination:\t{0}", t.GetDestin().GetName());
                    Console.WriteLine("Carrier to Complete:\t{0}", t.GetCarrier().GetName());
                }
                Console.WriteLine();
                iter++;
            }
            */

            if (TripsList.Count == 0)
                return null;
            else return TripsList[0]; //nothing fancy; return first option
        }

/*        private static List<Trip> OptimizeRoutes(List<List<Trip>> routes)
        {
            foreach(List<Trip> trips in routes)
            {
                double TripCost = 0.0;
                foreach(Trip t in trips)
                {
                    TripCost += t.GetTripCost();
                }
            }
        }*/

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
                return CarrierList.OneStopRoutes(CityList.GetCity(c1.GetName()), CityList.GetCity(c2.GetName()), 0,0);
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
         *  CarriersForRoute uses two separate algorithmic methods of determining if there are carriers available
         *  to fulfill an order.
         *  
         *  Firstly, the CarriersForRoute function assumes that using a single carrier is the best method of action.
         *  This is, broadly speaking, optimizing for time. If a carrier switch needs to occur, there must be time
         *  to unload and reload in the intermediary city. Thus we test for a single carrier solution first if at
         *  all possible.
         *  
         *  Secondly, if a single carrier solution is not found, we test for a two carrier solution. This algorithm
         *  successfully covers almost every possible order. We call the FindIntermediaryCity function, which returns
         *  a List of two Trip objects that the initial trip has now been split into.
         *  
         *  Thirdly, if there are multiple carrier options that can satisfy a single carrier trip, we just assume that
         *  the one that was found first is best and return it. Maybe I'll implement an algorithm that will optimize
         *  for cost effectiveness, but rn i have other stuff to do.
         *  
         *  This function effectively elminates the need for the Buyer to manually choose intermediary cities, because
         *  this function finds out if they're needed at all in the first place. Something to consider, however, is the
         *  methodology this function uses to determine which carrier combination to go with. There are likely many
         *  possible carrier combinations; a data set like this could be optimized for speed or cost. I don't have time
         *  to do this, but maybe over winter break for fun! :)
         *  
         *  TODO:
         *  
         *  If an order will take > 8h of driving (think CityList.DrivingDistance function!!!) we MUST separate into
         *  two orders. DO THIS SHIT
         *  
         *  probably as easy as calling that func on o.origin and o.destin usinga  conditional just think about it
         *  
         *  - Ryan Enns, 2021-12-03
         * PARM : Order
         * RTRN : Carrier
         */
        public static List<Trip> CarriersForRoute(Order o)
        {
            if (o == null) return null;

            //duped code but it's better than running thru the foreach loop for no reason
            if(CityList.DrivingTime(o.GetOrigin(), o.GetDestin()) > 8)
            {
                return CarrierList.OneStopRoutes(CityList.GetCity(o.GetOrigin()), CityList.GetCity(o.GetDestin()), o.mr.JobType, o.mr.Quantity);
            }

            List<Trip> t = new List<Trip>();
            foreach(Carrier c in Carriers)
            {
                //add each potential carrier to the list of potential carriers
                if (c.HasDepotIn(o.GetOrigin()) && c.HasDepotIn(o.GetDestin()))
                {
                    if (o.JobType() == "FTL")
                    {
                        if(c.GetDepot(o.GetOrigin()).HasFTLAvail(1))
                            t.Add(new Trip(o.GetOrigin(), o.GetDestin(), c));
                    }
                    else
                    {
                        if (c.GetDepot(o.GetOrigin()).HasLTLAvail(o.LTLQty()))
                            t.Add(new Trip(o.GetOrigin(), o.GetDestin(), c));
                    }
                }
            }

            if(t.Count == 0)
            {
                return CarrierList.OneStopRoutes(CityList.GetCity(o.GetOrigin()), CityList.GetCity(o.GetDestin()), o.mr.JobType, o.mr.Quantity);
            }
            else
            {
                List<Trip> ReturnValue = new List<Trip>();
                ReturnValue.Add(t[0]);
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
