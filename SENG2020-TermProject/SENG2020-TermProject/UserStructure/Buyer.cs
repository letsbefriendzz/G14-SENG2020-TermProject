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
            ContractMarketAccess cma = new ContractMarketAccess();
            return cma.GetAllMarketplaceRequests();
        }
    }
}
