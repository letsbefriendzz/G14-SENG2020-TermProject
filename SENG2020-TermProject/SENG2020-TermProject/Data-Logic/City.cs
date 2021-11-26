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

        public static City CityAt(int i)
        {
            if (i >= 0 && i <= 7)
                return CitySequence[i];
            else return null;
        }

        private static bool ContainsCity(String cn)
        {
            foreach (City c in CitySequence)
                if (c.CityName == cn) return true;
            return false;
        }

        private static int GetCityIndex(String cn)
        {
            for(int i = 0; i < CitySequence.Count; i++)
            {
                if (CitySequence[i].CityName == cn)
                    return i;
            }
            return -1;
        }

        public static int DistanceBetween(String c1, String c2)
        {
            //if both cities passed exist, continue. otherwise return -1
            if(ContainsCity(c1) && ContainsCity(c2))
            {
                //get the indices of both cities in the array
                int StartIndex = GetCityIndex(c1);
                int EndIndex = GetCityIndex(c2);
                //if the starting index is less than the ending index
                //or, if we're traversing left to right, west to east.
                //upwards in array indices, use a regular for loop.
                if(StartIndex < EndIndex)
                {   //moving east!
                    int Distance = 0;
                    for (int i = StartIndex; i < EndIndex; i++)
                        Distance += CitySequence[i].EastCityDistance;
                    return Distance;
                }
                //otherwise, we're moving east to west, or backwards,
                //or downwards in array indices.
                else
                {   //moving west!
                    int Distance = 0;
                    for (int i = StartIndex; i > EndIndex; i--)
                        Distance += CitySequence[i].WestCityDistance;
                    return Distance;
                }
            }
            return -1;
        }
    }
}
