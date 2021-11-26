/*
 * FILE             : MarketplaceRequest.cs
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

namespace SENG2020_TermProject
{
    /**
     * \brief       The MarketplaceRequest class is a data container for Contract Marketplace requests received from the Marketplace database.
     * 
     * \details     In the Contract Marketplace database, six fields are present and returned to the end users. This includes the name of the
     *              client, the type of job, the quantity of items in their order, the city of origin of the shipment, the shipment's destination,
     *              and the type of van required to get the job done.
     *              
     *              The MarketplaceRequest class groups all of this data together for an easy to use interface for our future code. This makes
     *              interfacing with the Contract Marketplace easier than having to parse the individual strings returned in DataTables.
     */
    class MarketplaceRequest
    {
        /**
         * \brief       ClientName represents a request's parent company.
         * 
         * \details     ClientName is a String that represents the name of the company that has issued
         *              the Marketplace Request that this instance represents.
         */
        public String ClientName { get; set; }

        /**
         * \brief       JobType
         * 
         * \details     JobType is an integer with two acceptable values. 0 indicates that the shipping
         *              is a full truck load, or FTL order. If the JobType integer is 1, this means that
         *              the order is a less than truck load order, or LTL order.
         */
        public int JobType { get; set; }

        /**
         * \brief       Quantity
         * 
         * \details     A quantity value of 0 indicates use of a full truck load.
         *              Any positive integer indicates LTL shipping of Quantity number of pallettes.
         */
        public int Quantity { get; set; }

        /**
         * \brief       CityOrigin represents the city that the shipment starts in.
         * 
         * \details     CityOrigin is a String that represents the present city that the shipment
         *              resides in.
         */
        public String CityOrigin { get; set; }

        /**
         * \brief       CityDestin represnts the city that the shipment will go to.
         * 
         * \details     CityDestin is a String that represnts the city that the shipment needs
         *              to be delivered to.
         */
        public String CityDestin { get; set; }

        /**
         * \brief       VanType represents the type of shipment vehicle required for this request.
         * 
         * \details     VanType has two acceptable values; 0 or 1. If a MarketplaceRequest's VanType
         *              is 0, it requires a dry van. If the VanType value is 1, it requries a Reefer.
         */
        public int VanType { get; set; }

        /**
         * \brief       Default MarketplaceRequest() Constructor
         * 
         * \details     The default MarketplaceRequest constructor sets all values to empty, non-null
         *              values. This means that all integers are -1 and all strings are a null terminator ("").
         */
        public MarketplaceRequest()
        {
            ClientName = "";
            JobType = -1;
            Quantity = -1;
            CityOrigin = "";
            CityDestin = "";
            VanType = -1;
        }

        /**
         * \brief       Extended MarketplaceRequest Constructor
         * 
         * \details     This MarketplaceRequest constructor takes various parameters to copy to the MarketplaceReqeust's
         *              various members. This includes the ClientName, JobType, Quantity, CityOrigin, CityDestin, and VanType.
         */
        public MarketplaceRequest(String cn, int jt, int q, String co, String cd, int vt)
        {
            this.ClientName = cn;
            this.JobType = jt;
            this.Quantity = q;
            this.CityOrigin = co;
            this.CityDestin = cd;
            this.VanType = vt;
        }

        /**
         * \brief   Dumps MarketplaceRequest object data to the consnole.. 
         *
         * \details This function displays the contents of each member variable of
         *          an instance of a MarkeplaceRequest object.
         *
         * \note    
         *
         * \param[in]     void
         *
         * \return        void
         */
        public void Display()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("ClientName:\t{0}", ClientName);
            Console.WriteLine("JobType:\t{0}", JobType);
            Console.WriteLine("Quantity:\t{0}", Quantity);
            Console.WriteLine("Origin:\t\t{0}", CityOrigin);
            Console.WriteLine("Destin:\t\t{0}", CityDestin);
            Console.WriteLine("VanType:\t{0}", VanType);
            Console.WriteLine();
        }
    }
}
