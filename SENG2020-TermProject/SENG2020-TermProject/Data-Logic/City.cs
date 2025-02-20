﻿/*
 * FILE             : City.cs
 * PROJECT          : SENG2020 Term Project
 * PROGRAMMER       : Ryan Enns
 * FIRST VERSION    : 2021-11-26
 * DESCRIPTION      :
 *  The City.cs file defines two classes; the City class and the CityList static class.
 *  The City object is used to represent a City in our supply chain. Since our supply
 *  chain is linear, thank the Good Lord Jesus himself, we can create a staic List<T>
 *  of City instances to represent our linear and relatively constant supply chain.
 *  
 *  THE CITY CLASS
 *  ==============
 *  
 *  The City object exists solely to serve the CityList class and the Depot class. It
 *  has no complicated methods; it's a nice wrapper for data. It has fields for the city
 *  name, the city to the east of it (because west -> east is considered to be a linear
 *  progression in our supply chain), the distance to the city east of it, and the time
 *  it takes to travel to the city east of it. Wow, that's a lotta easts. The City class
 *  has getters for each of these members, and only one constructor that takes values
 *  for each field upon instantiation.
 *  
 *  THE CITYLIST CLASS
 *  ==================
 *  
 *  As previously mentioned, thank the Holy Saviour Lord Jesus himself like 20x over
 *  that our supply route is exclusively linear. If it wasn't, I'd probably be curled
 *  up in the fetal position trying to figure this back end out on my own. Considering
 *  I did that multiple times trying to figure out a linear supply route back end, I
 *  am quite thankful.
 *  
 *  Anyhow, the CityList class. What is it? Why does it exist?
 *  
 *  Well, the CityList class is comprised of a static List<T> of City instances that is
 *  used to model our supply route. Using its various methods, we can measure the time
 *  needed to travel from city x to city y, the distance between city x and city y, check
 *  if city xyz is in the list, check how many stops would be needed between city x and y
 *  for an LTL order, and so forth.
 */

using System;
using System.Collections.Generic;

namespace SENG2020_TermProject.Data_Logic
{
    /**
     * \brief       Represents a city that can be shipped to or from.
     * 
     * \details     The City class represents a city that can be shipped to or from.
     *              It contains fields to represent the city's name, and the information
     *              regarding the next cities on its east and west route respectively.
     *              
     *              This class exists exclusively to serve the CityList static class,
     *              which creates and holds a constant sequence-list of the cities that
     *              can be shipped to or from.
     */
    public class City
    {
        /// \brief      The name of the City this object represents.
        private readonly String CityName;
        /// \brief      The name of the next city east of CityName.
        private readonly String EastCityName;
        /// \brief      The distance to the city east of CityName, in km.
        private readonly int EastCityDistance;
        /// \brief      The time it takes to travel to the city west of CityName, in hours.
        private readonly double TimeToEastCity;

        #region City Accessors & Mutators

        public String GetName()
        {
            return this.CityName;
        }

        public String GetEastCityName()
        {
            return this.EastCityName;
        }

        public int GetEastCityDistance()
        {
            return this.EastCityDistance;
        }

        public double GetTimeToEastCity()
        {
            return this.TimeToEastCity;
        }

        #endregion

        public City(String cn, String ecn, int ecd, double ttec)
        {
            this.CityName = cn;

            this.EastCityName = ecn;
            this.EastCityDistance = ecd;
            this.TimeToEastCity = ttec;
        }
    }

    /**
     * \brief       Defines the order and list of cities that can be shipped to and from.
     */
    public static class CityList
    {
        /**
         * \brief       A List<T> that holds the sequence of and information about cities.
         */
        private static readonly List<City> CitySequence = new List<City>();

        /**
         * \brief       The default CityList constructor.
         * 
         * \details     This constructor fills the CitySequence List<City> with the cities that
         *              can be shipped to and from, in East to West order. It also stores the
         *              distance and time-to-travel for the next east and west cities respectively.
         */
        static CityList()
        {
            //null <- Windsor -> London
            CitySequence.Add(new City("Windsor", "London", 191, 2.5));

            //Windsor <- London -> Hamilton
            CitySequence.Add(new City("London", "Hamilton", 128, 1.75));

            //London <- Hamilton -> Toronto
            CitySequence.Add(new City("Hamilton", "Toronto", 68, 1.25));

            //Hamilton <- Toronto -> Oshawa
            CitySequence.Add(new City("Toronto", "Oshawa", 60, 1.3));

            //Toronto <- Oshawa -> Belleville
            CitySequence.Add(new City("Oshawa", "Belleville", 134, 1.65));

            //Oshawa <- Belleville -> Kingston
            CitySequence.Add(new City("Belleville", "Kingston", 82, 1.2));

            //Belleville <- Kingston -> Ottawa
            CitySequence.Add(new City("Kingston", "Ottawa", 196, 2.5));

            //Kingston <- Ottawa -> null
            CitySequence.Add(new City("Ottawa", null, -1, -1.0));
        }

        /**
         * \brief       Returns the City object stored in the CitySequence at index i.
         * 
         * \details     Accesses the list via square bracket notation.
         * 
         * \retval      Returns a City object.
         */
        /*
         * NAME : CityAt
         * DESC :
         *  This function returns the city located at index i
         *  of the CitySequence List<T>.
         * RTRN : Coty
         * PARM : int
         */
        public static City CityAt(int i)
        {
            if (i >= 0 && i <= CitySequence.Count)
                return CitySequence[i];
            else return null;
        }

        /**
         * \brief       Checks if there is a City with the name passecd as an argument to the function.
         * 
         * \details     Iterates through each City object, comparing the city name passed to it to the respective City.CityName value.
         * 
         * \retval      Returns a boolean; true if the city is found, false if not.
         */
        /*
         * NAME : ContainsCity
         * DSEC :
         *  The ContainsCity function returns true or false depending on if a City
         *  with the name specified exists in the CityList.
         * RTRN : bool
         * PARM : String
         */
        public static bool ContainsCity(String cn)
        {
            foreach (City c in CitySequence)
                if (c.GetName() == cn) return true;
            return false;
        }

        /**
         * \brief       Returns the index of a given city based on the city name passed to the function.
         * \details     Iterates through the CitySequence via generic for loop; compares CitySequence[i].CityName with the passed city name.
         * 
         * \retval      Returns an integer; either the index that the city name is found at in the CitySequence, or -1 if it doesn't exist.
         */
        /*
         * NAME : GetCityIndex
         * DESC :
         *  This function returns the index of a city based on the name passed to it.
         *  If it doesn't exist, -1 is returned.
         * RTRN : int
         * PARM : String
         */
        public static int GetCityIndex(String cn)
        {
            for (int i = 0; i < CitySequence.Count; i++)
            {
                if (CitySequence[i].GetName() == cn)
                    return i;
            }
            return -1;
        }

        /*
         * NAME : GetCity
         * DESC
         *  The GetCity function returns a City instance that shares a CityName with the
         *  String passed to the function as a parameter.
         * RTRN : City
         * PARM : String
         */
        public static City GetCity(String city)
        {
            if (ContainsCity(city))
            {
                foreach (City c in CitySequence)
                    if (c.GetName() == city) return c;
            }
            return null;
        }

        /**
         * \brief       Calculates the <b>driving distance</b> between two cities.
         * 
         * \details     The DrivingDistance function accumulates the total distance
         *              between two given cities. It iterates through the CitySequence,
         *              accumulating the distance between the cities, and returns the
         *              total distance between them.
         * 
         * \retval      Returns an integer; if the cities passed do not exist, <b>-1</b>
         *              is returned as an error value.
         */
        /*
         * NAME : DrivingDistance
         * DESC :
         *  This function determines the total distance between two cities
         *  as defined by the values found in the CitySequence. The origin
         *  and destination cities are specified by city names in the form
         *  of String objects passed as parmaeters. If one of the city names
         *  is invalid, -1 is returned.
         * RTRN : int
         * PARM : String, String
         */
        public static int DrivingDistance(String c1, String c2)
        {
            //if both cities passed exist, continue. otherwise return -1
            if (ContainsCity(c1) && ContainsCity(c2))
            {
                //get the indices of both cities in the array
                int StartIndex = GetCityIndex(c1);
                int EndIndex = GetCityIndex(c2);

                if (StartIndex > EndIndex)
                {
                    int inter = StartIndex;
                    StartIndex = EndIndex;
                    EndIndex = inter;
                }

                int Distance = 0;
                for (int i = StartIndex; i < EndIndex; i++)
                    Distance += CitySequence[i].GetEastCityDistance();
                return Distance;
            }
            return -1;
        }

        /**
         * \brief       Calculates the <b>driving time</b> between two cities.
         * 
         * \details     The DrivingTime function accumulates the total distance
         *              between two given cities, assuming they exist.
         *              <br></br>
         *              
         *              <b>IMPORTANT TO NOTE :</b> <br></br>
         *              
         *              The DrivingTime function <b>only</b> calculates the time
         *              it takes to drive between two locations. This means that
         *              it <b>does not account for the 2 hour loading times at start
         *              and finish,</b> and it <b>does not account for LTL stops.</b>
         * 
         * \retval      Returns a double. If one of the cities
         *              passed to the function doesn't exist, it returns <b>-1.0</b> as
         *              an error code.
         */
        /*
         * NAME : DrivingTime
         * DESC :
         *  The DrivingTime function, like the DrivingDistance function, accumulates
         *  the time it takes to travel between an origin and destination city. The
         *  cities are defined by the two String city names passed as parameters. If
         *  either the origin or destination city doesn't exist in the CitySequence,
         *  -1 is returned as an error code.
         * RTRN : double
         * PARM : String, String
         */
        public static double DrivingTime(String c1, String c2)
        {
            if (ContainsCity(c1) && ContainsCity(c2))
            {
                //get the indices of both cities in the array
                int StartIndex = GetCityIndex(c1);
                int EndIndex = GetCityIndex(c2);

                //if the we need to traverse backwards, well, we won't!
                //just swap the values and be done with it
                if (StartIndex > EndIndex)
                {
                    int inter = StartIndex;
                    StartIndex = EndIndex;
                    EndIndex = inter;
                }

                double TimeToTravel = 0;
                for (int i = StartIndex; i < EndIndex; i++)
                    TimeToTravel += CitySequence[i].GetTimeToEastCity();
                return TimeToTravel;
            }
            return -1.0;
        }

        /**
         * \brief       Calculates the number of stops on a given route if the order is LTL.
         * 
         * \details     This function calculates the number of stops between two cities. It subtracts the
         *              lesser city index from the greater city index, and then subtracts one. That's it.   
         * 
         * \retval      Returns an integer, the number of stops between a two cities. If a city
         *              passed doesn't exist, -1 is returned.
         */
        /*
         * NAME : LTLStops
         * DESC :
         *  LTLStops takes the indices of an origin and destination city and counts
         *  how many array entries there are between them. If either cities passed
         *  as parameters don't exist, -1 is returned as an error code.
         * RTRN : int
         * PARM : String, String
         */
        public static int LTLStops(String c1, String c2)
        {
            if (ContainsCity(c1) && ContainsCity(c2))
            {
                int StartIndex = GetCityIndex(c1) + 1;
                int EndIndex = GetCityIndex(c2) + 1;

                if (StartIndex > EndIndex)
                    return (StartIndex - EndIndex) - 1;
                else
                    return (EndIndex - StartIndex) - 1;
            }
            return -1;
        }
    }
}
