/*
* FILE             : Order.cs
* PROJECT          : SENG2020 - Term Project
* PROGRAMMER(s)    : Ryan Enns
* FIRST VERSION    : 2021-11-25
* DESCRIPTION      :
*/

using System.Collections.Generic;

//reference this fucker like crazy!
//https://conestoga.desire2learn.com/d2l/le/content/482677/viewContent/9959524/View

namespace SENG2020_TermProject.Data_Logic
{
    /**
     * \brief       The Order class represents an order that can be created by a Buyer's request.
     * 
     * \details     Orders are created by Buyers. Orders are comprised of a Marketplace Request,
     *              a singular carrier or group of carriers who are available to fulfill this
     *              request, and finally an invoice that will be stored in the TMS database and
     *              be served to the customer.
     */
    class Order
    {
        /**
         * \brief       mr represents the MarketplaceRequest object that this Order was created from.
         * 
         * \details     As specified in the technical requirements of this software, Orders are created by
         *              Buyers. The Buyer requests contracts from the Contract Marketplace - the back end
         *              of which is specified under Communications/DatabaseManagement/ContractMarketAccess.
         *              The buyer can then process this contract into an order. Thus the cornerstone of the
         *              order is a MarketplaceRequest. The Order class has a private MarketplaceRequest
         *              member to represent the MarketplaceRequest from which the Order was derived.
         */
        private MarketplaceRequest mr = null;

        /// \brief      A boolean that indicates if this order has been fulfilled or not.
        public bool IsComplete;
        /// \brief      A float that represents how many hours the selected shipping sequence will take
        public double TimeToComplete;
        public int DistanceToComplete;

        public Order()
        {

        }
    }
}
