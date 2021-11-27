/*
 * FILE             : Users.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class User
    {
        /// \brief      Dictates whether a user can change database connection information.
        public bool CanChangeDatabaseConnections = false;
        /// \brief      Dictates whether a user can generate an invoice.
        public bool CanGenerateInvoice = false;
        /// \brief      Dictates whether a user can generate an order.
        public bool CanGenerateOrder = false;
        /// \brief      Dictates whether a user can use functions in the ContractMarketAccess class.
        public bool CanAccessContractMarket = false;
        /// \brief      Dictates whether a user can use functions in the TMSDatabaseAccess class.
        public bool CanAccessTMSDatabase = false;
    }
}
