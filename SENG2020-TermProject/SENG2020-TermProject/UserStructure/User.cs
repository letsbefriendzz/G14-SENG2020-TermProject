/*
 * FILE             : Users.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  The User.cs file defines a generic User object that the Planner, Buyer, and Admin
 *  inherit from. This prevents code duplication, as each has access to the TMSDatabase
 *  and needs some generic console UI components that can be provided through static
 *  methods present in the class.
 */

using System;
using System.Threading;
using SENG2020_TermProject.DatabaseManagement;
/*
 * Various users can do various different things. The only way that these users interact is via fetching data
 * from the TMS database. They exclusively access the end results of each other's labours through the TMS database.
 * The Buyer initializes the new order and sends it to the database. The Planner then gets all unfulfilled orders
 * from the database to select a carrier route - they then simulate and confirm the order is finished, and then
 * return the order to the database along with the invoice.
 */
namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       The abstract class from which user types are derived.
     * 
     * \details     In the TMS system, there are three user levels; Admin, Buyer
     *              and Planner. These user types can all derive from a common
     *              basis, in the form of an abstract User class. The User class
     *              defines common fields and methods that each of its subclasses
     *              uses.
     */
    class User
    {
        //renns
        protected TMSDatabaseAccess tms;
        public static String GetInput()
        {
            Console.Write(">> ");
            return Console.ReadLine();
        }

        public static void Delay(int delay = 250)
        {
            Thread.Sleep(delay);
        }

        public static void ClearTerminal()
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        }

        public static int GetIntBetween(int UpperBound, int LowerBound = 0)
        {
            String inp = null;
            int ReturnValue = -1;
            while (ReturnValue < LowerBound || ReturnValue > UpperBound)
            {
                inp = "";
                while (!int.TryParse(inp, out ReturnValue))
                {
                    Console.WriteLine("Enter an integer between {0} and {1}", LowerBound, UpperBound);
                    inp = GetInput();
                }
            }

            return ReturnValue;
        }

        public static String GetYesNo()
        {
            String inp = "";
            while (inp != "Y" && inp != "N")
            {
                inp = GetInput().ToUpper();
            }

            return inp;
        }
    }
}
