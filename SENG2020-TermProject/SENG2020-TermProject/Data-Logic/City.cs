/*
 * FILE             : City.cs
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
    class City
    {
        /// \brief      The name of the City this object represents.
        public String CityName;

        /// \brief      The name of the next city west of CityName.
        public String WestCityName;
        /// \brief      The distance to the city west of CityName, in km.
        public int WestCityDistance;
        /// \brief      The time it takes to travel to the city west of CityName, in hours.
        public double TimeToWestcity;

        /// \brief      The name of the next city east of CityName.
        public String EastCityName;
        /// \brief      The distance to the city east of CityName, in km.
        public int EastCityDistance;
        /// \brief      The time it takes to travel to the city west of CityName, in hours.
        public double TimeToEastCity;

        public City()
        {

        }

        public City(String cn, String wcn, int wcd, double ttwc, String ecn, int ecd, double ttec)
        {
            this.CityName = cn;
            
            this.WestCityName = wcn;
            this.WestCityDistance = wcd;
            this.TimeToWestcity = ttwc;

            this.EastCityName = ecn;
            this.EastCityDistance = ecd;
            this.TimeToEastCity = ttec;
        }
    }

    /**
     * \brief       Defines the order and list of cities that can be shipped to and from.
     */
    static class CityList
    {
        /**
         * \brief       A List<T> that holds the sequence of and information about cities.
         */
        public static List<City> CitySequence = new List<City>();

        /**
         * \brief       The default CityList constructor.
         * 
         * \details     This constructor fills the CitySequence List<City> with the cities that
         *              can be shipped to and from, in East to West order. It also stores the
         *              distance and time-to-travel for the next east and west cities respectively.
         */
        static CityList()
        {
            //really this list is duplicating data but, I do not care! ease of access please!

            //null <- Windsor -> London
            CitySequence.Add(new City("Windsor", null, -1, -1.0, "London", 191, 2.5));

            //Windsor <- London -> Hamilton
            CitySequence.Add(new City("London", "Windsor", 191, 2.5, "Hamilton", 128, 1.75));

            //London <- Hamilton -> Toronto
            CitySequence.Add(new City("Hamilton", "London", 128, 1.75, "Toronto", 68, 1.25));

            //Hamilton <- Toronto -> Oshawa
            CitySequence.Add(new City("Toronto", "Hamilton", 68, 1.25, "Oshawa", 60, 1.3));

            //Toronto <- Oshawa -> Belleville
            CitySequence.Add(new City("Oshawa", "Toronto", 60, 1.3, "Belleville", 134, 1.65));

            //Oshawa <- Belleville -> Kingston
            CitySequence.Add(new City("Belleville", "Oshawa", 134, 1.65, "Kingston", 82, 1.2));

            //Belleville <- Kingston -> Ottawa
            CitySequence.Add(new City("Kingston", "Belleville", 82, 1.2, "Ottawa", 196, 2.5));

            //Kingston <- Ottawa -> null
            CitySequence.Add(new City("Ottawa", "Kingston", 196, 2.5, null, -1, -1.0));
        }

        /**
         * \brief       Dumps list info to the console.
         */
        public static void DisplayList()
        {
            foreach(City c in CitySequence)
            {
                Console.WriteLine(c.CityName);
            }
        }

        /**
         * \brief       Returns the City object stored in the CitySequence at index i.
         * 
         * \details     Accesses the list via square bracket notation.
         * 
         * \retval      Returns a City object.
         */
        public static City CityAt(int i)
        {
            if (i >= 0 && i <= 7)
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
        private static bool ContainsCity(String cn)
        {
            foreach (City c in CitySequence)
                if (c.CityName == cn) return true;
            return false;
        }

        /**
         * \brief       Returns the index of a given city based on the city name passed to the function.
         * \details     Iterates through the CitySequence via generic for loop; compares CitySequence[i].CityName with the passed city name.
         * 
         * \retval      Returns an integer; either the index that the city name is found at in the CitySequence, or -1 if it doesn't exist.
         */
        private static int GetCityIndex(String cn)
        {
            for(int i = 0; i < CitySequence.Count; i++)
            {
                if (CitySequence[i].CityName == cn)
                    return i;
            }
            return -1;
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
        public static int DrivingDistance(String c1, String c2)
        {
            //if both cities passed exist, continue. otherwise return -1
            if(ContainsCity(c1) && ContainsCity(c2))
            {
                //get the indices of both cities in the array
                int StartIndex = GetCityIndex(c1);
                int EndIndex = GetCityIndex(c2);

                if(StartIndex > EndIndex)
                {
                    int inter = StartIndex;
                    StartIndex = EndIndex;
                    EndIndex = inter;
                }

                int Distance = 0;
                for (int i = StartIndex; i <= EndIndex; i++)
                    Distance += CitySequence[i].EastCityDistance;
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
        public static double DrivingTime(String c1, String c2)
        {
            if(ContainsCity(c1) && ContainsCity(c2))
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
                for (int i = StartIndex; i <= EndIndex; i++)
                    TimeToTravel += CitySequence[i].TimeToEastCity;
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
        public static int LTLStops(String c1, String c2)
        {
            if(ContainsCity(c1) && ContainsCity(c2))
            {
                int StartIndex = GetCityIndex(c1)+1;
                int EndIndex = GetCityIndex(c2)+1;

                if (StartIndex > EndIndex)
                    return (StartIndex - EndIndex) - 1;
                else
                    return (EndIndex - StartIndex) - 1;
            }
            return -1;
        }
    }
}
