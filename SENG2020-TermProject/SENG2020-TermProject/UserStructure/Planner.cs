/*
 * FILE             : Planner.cs
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
The Planner employee is responsible for furthering the order by selecting one or more registered 
Carriers to fulfill the Order, in the form of Trips. Once assigned, the Planner monitors the progression of
time in the application, ensuring that when all Trips on an order are completed, the Order will be 
marked as Completed and sent back to the Buyer for Invoice Generation. Finally, the Planner may also 
produce reports showing aggregate activity in OSHT.

4.5.2.3.1 Planner receives Orders from the Buyer.

4.5.2.3.2 Planner selects Carriers from the targeted cities to complete the Order, which adds a 
‘Trip’ to the Order for each Carrier selected

4.5.2.3.3 Carriers may be limited in their transportation capacity, thus the Planner ensures that 
multiple Trips, if necessary are attached to the Order.

4.5.2.3.4 The Planner may simulate the passage of time in 1-day increments in order to mover 
Orders and their trips to completed state

4.5.2.3.5 Planner may confirm an order is completed. Completed Orders are marked for follow-up 
from the Buyer

4.5.2.3.6 Planner may see a summary of all active Orders in a status screen.

4.5.2.3.7 (Optional) The status screen is highly graphical, perhaps using some kind of Google Maps 
plugin.

4.5.2.3.8 Planner may generate a summary report of all Invoice data for a) all time, and b) The 
‘past 2 weeks’ of simulated ti
 */
namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       Defines the permissions and abilities of the Planner user type.
     * 
     * \details     The Planner is responsible for furthering created orders by selecting
     *              a carrier or carriers to fulfill it. The planner also keeps track of
     *              order progress.
     */
    public class Planner : User
    {

    }
}
