/*
 * FILE             : _MainProgram.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  This is the main thread function of my implementation of the OSHT TMS.
 *  Here we select a workflow, Admin, Buyer or Planner.
 *  
 *  Important to note - everywhere in this sln that a file header only lists Ryan Enns
 *  as the programmer, every line of code and every class, function, and file header
 *  within it has been written by me (Ryan Enns) exclusively.
 */

//2906 lines of code as of 2021-12-04 -- 5:03pm

//makes sense to define the DOxygen index stuff where main() can be found.
/**
 * \mainpage Transportation Management System
 * 
 * \section         Functional      Functional Requirements
 * 
 * The OSHT Transportation Management System must allow various user types to manage the logistics
 * of shipping sequences between various Ontario cities. This is achieved through accessing a Contract
 * Marketplace, where valid carrier information is then presented. The TMS user can then choose the
 * carrier they'd like, simulate passing time, and then declare an order fulfilled. An invoice can
 * be generated and the Order is updated.
 * 
 * \section         ToDoList        To Do list
 * 
 * \subsection      Configure Carrier Update System
 * 
 * What is it? How does it work? Is it just Carriers.csv with us reading and writing to the file based
 * on orders we take and complete?
 * 
 * \subsection      Multi-Carrier Orders
 * 
 * Can an order be satisfied by multiple carriers? If so, does it require that the product be dropped
 * at the respective carrier's closest depot, only to then be picked up by another carrier to its final
 * destination?
 * <br></br>
 * What do we do with orders that can't be satisfied by a single carrier service?
 * 
 * \subsection      Start Writing WPF UI
 * 
 * For the functioning prototype, I (Ryan) will be making a console UI to work with. However, requirements
 * state that a WPF GUI is required - this needs to be created. However, it should be relatively easy to
 * snap this on top of the functioning program if a functioning console UI is already largely working.
 * 
 * \section         Database        Database Requirements
 * 
 * There are 3 databases accessed by the TMS; the Contract Marketplace, the Carriers Update System
 * flatfile, and the local TMS database.
 * 
 * \subsection  Contract Marketplace
 * 
 * The Contract Marketplace is accessed by the ContractMarketAccess class, a class that extends the
 * abstract DatabaseAccess class. The ContractMarketAccess aims to provide an easy to use interface
 * for the Buyer users to request varying types of Contracts. This includes getting just LTL or FTL
 * contracts, Reefer contracts, contracts based on location, or just getting all available contracts.
 * 
 * \subsection  Carrier Update System
 * 
 * How exactly this works I don't know. note to self -- ask laura lol
 * 
 * \subsection  Local TMS Database
 * 
 * The local TMS database stores information about customers, open and closed orders, employees, and
 * other data.
 */

//here is the SET file header template:
/*
 * FILE             :
 * PROJECT          :
 * PROGRAMMER       :
 * FIRST VERSION    :
 * DESCRIPTION      :
 */

/*
 * TODO
 * 1. Work on GetDepots function in TMSDatabaseAccess
 * 2. Allow Planner to select from carrier options
 * 3. Establish most time efficient and cost efficient carrier options
 * 5. Migrate to Carrier db
 * 
 */

using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Communications;
using SENG2020_TermProject.Data_Logic;
using SENG2020_TermProject.UserStructure;
using System;
using System.Collections.Generic;

namespace SENG2020_TermProject
{
    class _MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            FileAccess.initInstallDirectories();

            UserFlowHarness();

            AnyKeyToContinue();
            return; //and then return!
        }








        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //VARIOUS TEST HARNESS FUNCTIONS
        static void TestCities()
        {
            foreach (City c1 in CityList.GetList())
            {
                foreach (City c2 in CityList.GetList())
                {
                    /*                    
                    if (CarrierList.CarriersForRoute(c1, c2) == null)
                    {
                        Console.WriteLine("Bad Test");
                        Console.WriteLine("Origin:\t{0}", c1.GetName());
                        Console.WriteLine("Destin:\t{0}", c2.GetName());
                        Console.WriteLine();
                    }
                    */
                }
            }
        }

        static void UserFlowHarness()
        {
            String inp = null;
            Console.WriteLine("Select Buyer (1) or Planner (2)");
            Console.Write(">> ");

            while (inp == null)
            {
                inp = Console.ReadLine();
                if (inp == "1")
                {
                    Buyer b = new Buyer();
                    b.BuyerWorkFlow();
                }
                else if (inp == "2")
                {
                    Planner p = new Planner();
                    p.PlannerWorkFlow();
                }
                else
                {
                    inp = null;
                }
            }
        }
    }
}