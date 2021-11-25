///<summary>
/// This is a test of the Summary meme in doxygen.
/// </summary>
/// <remarks>
/// idk what this does!
/// </remarks>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENG2020_TermProject
{
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
