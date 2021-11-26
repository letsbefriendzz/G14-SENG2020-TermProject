using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SENG2020_TermProject.UserStructure;

namespace TMSConsoleUI
{
    class UI
    {
        private User u;

        public UI()
        {
            u = null;
            init();
        }

        public void init()
        {
            Console.WriteLine("What type of user would you like to log in as?");
            Console.WriteLine("1. Admin\n2. Buyer\n3. Planner");
            String input = "";
            do
            {
                input = Console.ReadLine();
            } while (input != "1" && input != "2" && input != "3");

            if (input == "1")
                u = new Admin();
            else if (input == "2")
                u = new Buyer();
            else if (input == "3")
                u = new Planner();
        }

        public void Run()
        {

        }
    }
}
