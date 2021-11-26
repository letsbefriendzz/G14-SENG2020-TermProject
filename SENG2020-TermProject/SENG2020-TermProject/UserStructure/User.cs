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
