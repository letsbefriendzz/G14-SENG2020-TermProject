/*
 * FILE             : Admin.cs
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
4.5.2.1 Admin

The Admin user role represents an individual at OSHT who has IT experience and is tasked with 
configuration, maintenance, and troubleshooting of the TMS application.

4.5.2.1.1 Admin may access general configuration options for TMS, such as selecting directories 
for log files, targeting IP address and ports for all DBMS communications; etc.

4.5.2.1.2 Admin may review logfiles without leaving the TMS application.

4.5.2.1.3 Admin may alter the following, key TMS data:
 - Rate/Fee Tables
 - Carrier Data (e.g. to add, update, or delete Carrier information)
 - Route Table

4.5.2.1.4 Admin may initiate a backup job on the local TMS Database, specifying the directory for 
the backup files to be produced.
 */
namespace SENG2020_TermProject.UserStructure
{
    /**
     * \brief       The Admin is the root user of the TMS system.
     * 
     * \details     The Admin can modify all configuration options for TMS. They can
     *              modify log directories, database IP addresses, database ports,
     *              review logs, alter carrier data and the route table, among others.
     */
    class Admin : User
    {
        public void AdminWorkFlow()
        {

        }
    }
}
