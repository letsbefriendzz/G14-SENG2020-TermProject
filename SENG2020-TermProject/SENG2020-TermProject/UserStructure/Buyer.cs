/*
 * FILE             : Buyer.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 */

using SENG2020_TermProject.DatabaseManagement;
using SENG2020_TermProject.Data_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * The buyer represents an employee of OSHT who is tasked with requesting Customer contracts from the 
    Contract Marketplace and generating an initial Order or contract. Their chief output is an Order, which 
    is marked for action by the Planner. After the Planner’s work is completed, the Buyer confirms each 
    completed Order and generates an Invoice to the Customer.

    4.5.2.2.1 Initiates contact with the Contract Marketplace to receive contracts from Customers.

    4.5.2.2.2 Buyer may review existing Customers and accept new Customers (from the Marketplace)
    into the TMS system

    4.5.2.2.3 Buyer may initiate a new Order from the Marketplace requests.

    4.5.2.2.4 Buyer may select relevant Cities for the Order. This will nominate Carriers in those Cities 
    for Order completion (which is confirmed by the Planner)

    4.5.2.2.5 Buyer may review completed Orders and process them for Invoice Generation.

    4.5.2.2.6 Invoice Generation results in a text file being generated with appropriate billing details. 
    This information is also stored in the TMS database.
 */
namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       Defines the permissions and abilities of the Buyer user type.
     * 
     * \details     The Buyer is an OSHT employee who is responsible for retrieving
     *              contracts from the marketplace and generating unfilled orders for
     *              the Planner to further process. The Buyer, once an order has been
     *              fulfilled, will generate an invoice for the customer and the TMS
     *              database.
     */
    public class Buyer : User
    {
        public Buyer()
        {

        }

        public MarketplaceRequest[] GetContracts()
        {
            return new ContractMarketAccess().GetAllMarketplaceRequests();
        }
    }
}
