/*
 * FILE             : Carrier.cs
 * PROJECT          : SENG2020 Term Project
 * PROGRAMMER       : Ryan Enns
 * FIRST VERSION    : 2021-11-26
 * DESCRIPTION      :
 *  Ah yes, the Carrier.cs file. Finally getting aroud to writing my documentation for it a little
 *  more thoroughly than I have thusfar. I'm in the midst of writing lots of documenation today 
 *  (2021-12-04), so my apologies for any typos or grammar errors.
 *  
 *  The Carrier.cs file actually defines three classes. Carrier, Depot, and CarrierList. We will
 *  go through these in descending order.
 *  
 *  funny how my lines get progressively longer as I type, it's like I never learned passed gr 2 lol
 *  
 *  THE CARRIER CLASS
 *  =================
 *  
 *  The Carrier class is used to define a Carrier that can fulfill an order's needs. A carrier contains                                        <- the line started this short
 *  only two fields, the name of the Carrier as defined by a String and a List<T> object of Depot objects
 *  used to contain the information about the various cities that the Carrier has depots in.
 *  
 *  The City class is short and to the point, as it exports most of its logic and data storage to the Depot
 *  class and other functions. It contains a getter for the CarrierName and getter for the Depots, as well
 *  as a function to display an instance's info to the console. It has two GetDepot methods for acquiring
 *  a Depot object if it is available, and a HasDepot function to determine if the Carrier has a depot in
 *  a given city.
 *  
 *  THE DEPOT CLASS
 *  ===============
 *  
 *  The Depot class exists exclusively as a handy dandy carrier for data that the Carrier class uses. Since
 *  a Carrier can have many Depots, it makes more sense to package all that potentially duplicate data up
 *  and put it in its own class to easily associate it all.
 *  
 *  At present, all members of this function are public. This will hopefully change to getters and setters.
 *  However, this leaves the Depot class with very few methods. Beyond some overloaded constructors, it has
 *  two functions to determine if the Depot has FTL or LTL availabilty.
 *  
 *  THE CarrierList CLASS
 *  =====================
 *  
 *  Finally, the bread and butter of the whole back end of this program, the CarrierList class. Like the other
 *  List classes in this solution, the CarrierList is a static class, and contains all information and methods
 *  pertaining to Carrier interaction and route establishment.
 *  
 *  The primary method of the CarrierList is, of course, the List<T> of Carriers that is set to all default
 *  values upon startup of the program. This is subject to change, however I've yet to migrate the CarrierList
 *  class to db dependent data fields and am leaving the hardcoded stuffs for now. I might just leave the hard
 *  coded values for the final solution out of convenience and lack of time. If you're wondering, yes, it's
 *  still Ryan as the sole dev on this project as of 2pm on the 4th. And I can't work on it past noon on the 7th.
 *  Oh Laura, if you're reading this, have pity on my measly console app solution.
 *  
 *  The true gem of the CarrierList class are the functions that set up Trip Lists that can be assigned to Orders.                          <- and ended this long lol
 *  This is managed by CarriersForRoute, SingleCarrierRoutes, and DoubleCarrierRoutes. The CarriersForRoute
 *  method combines the output of the SingleCarrierRoutes and DoubleCarrierRoutes functions to satisfy any possible
 *  order.
 *  
 *  Aside from those two functions, which I insist are the bread and butter of this whole application, the class
 *  also has methods to retrieve a given Carrier object and a Depot instance from a Carrier object stored within
 *  the CityList List<T>
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

        /*
         * NAME : Decrement
         * DESC :
         *  This function decrements either FTL or LTL availability. This function is called
         *  only when an order is executed by the planner. This ensures that the values stored
         *  in the various Carrier instances in the CarrierList class are updated.
         *  
         *  The function takes the job type integer to specify which field to decrement, and
         *  if the order is LTL the user can supply the quantity of pallets. the quantity field
         *  has a default value of 0.
         * RTRN : void
         * PARM : int, int = 0
         */
        public void Decrement(int JobType, int Quantity = 0)
        {
            if(JobType == 0)
            {
                this.FTLAvail--;
            }
            else
            {
                this.LTLAvail -= Quantity;
            }
        }

        /*
         * NAME : Increment
         * DESC :
         *  This function increments either FTL or LTL availability. This function is called
         *  only when an order is executed by the planner. This ensures that the values stored
         *  in the various Carrier instances in the CarrierList class are updated.
         *  
         *  The function takes the job type integer to specify which field to increment, and
         *  if the order is LTL the user can supply the quantity of pallets. the quantity field
         *  has a default value of 0.
         * RTRN : void
         * PARM : int, int = 0
         */
        public void Increment(int JobType, int Quantity = 0)
        {
            if (JobType == 0)
            {
                this.FTLAvail++;
            }
            else
            {
                this.LTLAvail += Quantity;
            }
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
    class CarrierList
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

        public static bool DecrementAvailability(String CarrierName, String DepotLocation, int JobType, int Quantity = 0)
        {
            foreach(Carrier c in Carriers)
            {
                if(c.GetName() == CarrierName)
                {
                    foreach(Depot d in c.Depots)
                    {
                        if(d.CityName == DepotLocation)
                        {
                            d.Decrement(JobType, Quantity);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IncrementAvailability(String CarrierName, String DepotLocation, int JobType, int Quantity = 0)
        {
            foreach (Carrier c in Carriers)
            {
                if (c.GetName() == CarrierName)
                {
                    foreach (Depot d in c.Depots)
                    {
                        if (d.CityName == DepotLocation)
                        {
                            d.Increment(JobType, Quantity);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /*
         * NAME : DoubleCarrierRoutes
         * DESC :
         *  FindIntermediaryCity takes two City objects and collects all potential two carrier solutions.
         *  .
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
         * RTRN : List<List<Trip>>
         * PARM : City, City
         */
        public static List<List<Trip>> DoubleCarrierRoutes(City c1, City c2, int JobType, int quantity = 0)
        {
            List<List<Trip>> TripsList = new List<List<Trip>>();
            String origin = c1.GetName();
            String destin = c2.GetName();
            int i1 = CityList.GetCityIndex(origin) +1;
            int i2 = CityList.GetCityIndex(destin);
            
            if(i1 > i2)
            {
                int inter = i1;
                i1 = i2;
                i2 = inter;
            }

            if (i1 == i2) return null;
            else
            {
                for (int i = i1; i < i2; i++)
                {
                    String CurrentCity = CityList.CityAt(i).GetName();

                    foreach (Carrier carrier1 in Carriers)
                    {
                        foreach (Carrier carrier2 in Carriers)
                        {
                            if (carrier1.CarrierName != carrier2.CarrierName && CurrentCity != origin && CurrentCity != destin)
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
            }

            if (TripsList.Count == 0)
                return null;
            else return TripsList; //nothing fancy; return first option
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
         * NAME : CarriersForOrder
         * DESC :
         *  The CarriersForOrder function calls on two functions, the SingleCarrierRoutes function and the
         *  DoubleCarrierRoutes function to determine all available carrier routes that could satisfy an order.
         *  This function either returns just single or double carrier routes if the opposite returns null,
         *  or it returns a combination of both lists.
         *  
         *  - Ryan Enns, 2021-12-03
         * PARM : Order
         * RTRN : Carrier
         */
        public static List<List<Trip>> CarriersForOrder(Order o)
        {
            if (o == null) return null;

            List<List<Trip>> SingleCarrierTrips = SingleCarrierRoutes(o);
            List<List<Trip>> DoubleCarrierTrips = CarrierList.DoubleCarrierRoutes(CityList.GetCity(o.GetOrigin()), CityList.GetCity(o.GetDestin()), o.mr.GetJobType(), o.mr.GetQuantity());

            List<List<Trip>> AllRoutes = new List<List<Trip>>();

            if (SingleCarrierTrips == null && DoubleCarrierTrips == null)
                return null;
            else if (SingleCarrierTrips == null && DoubleCarrierTrips != null)
                return DoubleCarrierTrips;
            else if (SingleCarrierTrips != null && DoubleCarrierTrips == null)
                return SingleCarrierTrips;
            else
            {
                foreach (List<Trip> route in SingleCarrierTrips)
                {
                    AllRoutes.Add(route);
                }

                foreach (List<Trip> route in DoubleCarrierTrips)
                {
                    AllRoutes.Add(route);
                }

                return AllRoutes;
            }
        }

        /*
         * NAME : SingleCarrierRoutes
         * DESC :
         *  This function, using an Order object as its reference point, determines if the route between
         *  the Order's origin city and destination city can be fulfilled by a single carrier with depots
         *  in both cities. Each potential route is added to a List of Trip Lists, and returned.
         *  
         *  Why return a List of Trip Lists? Well, because when we're working with the potential for multi
         *  carrier route solutions, it's easiest to group the single carrier solutions into the same best 
         *  format (List of Trip Lists) that's available for multi carrier routes. This allows all possible
         *  routes to be condensed into a single List of Trip Lists that can be returned and chosen from by
         *  the Planner.
         * RTRN : List<List<Trip>>
         * PARM : Order
         */
        public static List<List<Trip>> SingleCarrierRoutes(Order o)
        {
            List<List<Trip>> trips = new List<List<Trip>>();
            foreach (Carrier c in Carriers)
            {
                //add each potential carrier to the list of potential carriers
                if (c.HasDepotIn(o.GetOrigin()) && c.HasDepotIn(o.GetDestin()))
                {
                    if (o.JobType() == "FTL")
                    {
                        if (c.GetDepot(o.GetOrigin()).HasFTLAvail(1))
                        {
                            List<Trip> t = new List<Trip>();
                            t.Add(new Trip(o.GetOrigin(), o.GetDestin(), c));
                            trips.Add(t);
                        }
                    }
                    else
                    {
                        if (c.GetDepot(o.GetOrigin()).HasLTLAvail(o.LTLQty()))
                        {
                            List<Trip> t = new List<Trip>();
                            t.Add(new Trip(o.GetOrigin(), o.GetDestin(), c));
                            trips.Add(t);
                        }
                    }
                }
            }

            if (trips.Count == 0)
                return null;
            else return trips;
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
