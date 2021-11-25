using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SENG2020_TermProject
{
    class MainProgram
    {
        private static void AnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            ContractMarketAccess cma = new ContractMarketAccess();
            MarketplaceRequest[] mpr = cma.GetAllMarketplaceRequests();
            foreach(MarketplaceRequest mr in mpr)
            {
                mr.Display();
            }
            AnyKeyToContinue();
        }
    }
}
