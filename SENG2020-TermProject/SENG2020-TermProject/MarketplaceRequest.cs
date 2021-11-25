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
        public String ClientName { get; set; }
        public int JobType { get; set; }
        public int Quantity { get; set; }
        public String CityOrigin { get; set; }
        public String CityDestin { get; set; }
        public int VanType { get; set; }

        public MarketplaceRequest()
        {
            ClientName = "";
            JobType = -1;
            Quantity = -1;
            CityOrigin = "";
            CityDestin = "";
            VanType = -1;
        }

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
