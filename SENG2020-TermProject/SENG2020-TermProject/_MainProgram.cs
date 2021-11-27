/*
 * FILE             : MainProgram.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

/**
 * \mainpage SENG2020 Group 14 - TMS System
 * 
 * \section
 * 
 * \subsection
 * 
 * \section
 * 
 * \subsection
 */

//here is the SET file header template:
/*
 * FILE             :
 * PROJECT          :
 * PROGRAMMER       :
 * FIRST VERSION    :
 * DESCRIPTION      :
 */

using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Data_Logic;
using System;

namespace SENG2020_TermProject
{
    class _MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }


        //what is this doing?
        static void Main(string[] args)
        {
            MarketplaceRequest mp = new ContractMarketAccess().GetAllMarketplaceRequests()[0];
            mp.Display();

            AnyKeyToContinue();

            Order o = new Order(mp);
            o.Display();

            AnyKeyToContinue();

            return; //and then return!
        }
    }
}

/*
            String c1 = "Windsor";
            String c2 = "Ottawa";
            Console.WriteLine("Getting distance between {0} and {1}.\nDistance:\t{2}", c1, c2, CityList.DrivingDistance(c1,c2));
            Console.WriteLine("Getting driving time between {0} and {1}.\nTime:\t\t{2}", c1, c2, CityList.DrivingTime(c1, c2));

            Console.WriteLine("Stops between {0} and {1}:\t{2}",c1,c2, CityList.LTLStops(c1, c2));
            //good question!
            //we're making a new cma object and getting all available requests from the marketplace database.
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
            //and for each one we receive, we dump out the contents
            if(mpr != null)
            {
                foreach (MarketplaceRequest mr in mpr)
                    mr.Display();
            }

            //then we get any key to continue
            AnyKeyToContinue();

            CityList.DisplayList();

            AnyKeyToContinue();
 */