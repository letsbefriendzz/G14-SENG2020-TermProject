/*
 * FILE             : MainProgram.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

//here is the SET file header template:
/*
 * FILE             :
 * PROJECT          :
 * PROGRAMMER       :
 * FIRST VERSION    :
 * DESCRIPTION      :
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SENG2020_TermProject.DatabaseManagement;

namespace SENG2020_TermProject
{
    class MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }


        //what is this doing?
        static void Main(string[] args)
        {
            //good question!
            //we're making a new cma object and getting all available requests from the marketplace database.
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
            //and for each one we receive, we dump out the contents
            foreach(MarketplaceRequest mr in mpr)
            {
                mr.Display();
            }
            //then we get any key to continue
            AnyKeyToContinue();
            return; //and then return!
        }
    }
}
