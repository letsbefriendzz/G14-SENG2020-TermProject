/*
 * FILE             : Admin.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
 * DESCRIPTION      :
 *  This 
 */

using SENG2020_TermProject.Communications;
using SENG2020_TermProject.Data_Logic;
using SENG2020_TermProject.DatabaseManagement;
using System;
using System.Net;
/*
4.5.2.1 Admin

The Admin user role represents an individual at OSHT who has IT experience and is tasked with 
configuration, maintenance, and troubleshooting of the TMS application.

    4.5.2.1.1 Admin may access general configuration options for TMS, such as selecting directories 
    for log files, targeting IP address and ports for all DBMS communications; etc.

    -- Satisfied - the Admin can edit Contract Marketplace database access values and modify them.
    Admin can also view and modify the working directory of the application.

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
        private static void AdminHeader()
        {
            Console.WriteLine("===================");
            Console.WriteLine("Logged in as: Admin");
            Console.WriteLine("===================");
        }

        private void GetDatabaseAccess()
        {
            if (this.tms != null) return;

            Console.WriteLine("Enter the Admin TMS Database password: ");
            tms = new TMSDatabaseAccess(GetInput());
        }

        public void AdminWorkFlow()
        {
            AdminHeader();
            GetDatabaseAccess();
            Console.WriteLine("\n\n");
            if (!this.tms.ValidConnection)
            {
                Console.WriteLine("Invalid TMS Database username or password!");
                FileAccess.Log("Invalid Login Attempt by Admin");
                return;
            }

            FileAccess.Log("Administrator login.");
            String inp = "";
            while (inp != null)
            {
                Console.WriteLine("Make a selection:");
                Console.WriteLine("1. Configure Contract Database Settings");
                Console.WriteLine("2. Review Log Files");
                Console.WriteLine("3. Configure Log/Invoice Directory");
                Console.WriteLine("4. Alter Carrier Data");
                Console.WriteLine("5. Alter Route Table");
                Console.WriteLine("6. Back Up TMS Database");
                Console.WriteLine("0. Exit");

                inp = GetInput();
                Console.WriteLine();

                if (inp == "1")
                {
                    while (inp != null)
                    {
                        Console.WriteLine("Contract Marketplace DB Settings:");
                        Console.WriteLine("1. Change CMDB Server IP");
                        Console.WriteLine("2. Change CMDB Server Port");
                        Console.WriteLine("3. Change CMDB Username");
                        Console.WriteLine("4. Change CMDB Password");
                        Console.WriteLine("5. Change CMDB Table");
                        Console.WriteLine("0. Return");

                        inp = GetInput();
                        String NewValue = "";

                        if (inp == "1")
                        {
                            Console.WriteLine("Current Server IP:\t\t{0}", CMDatabaseValues.server);
                            Console.WriteLine("Enter a new IP:");
                            NewValue = GetInput();

                            IPAddress Test;

                            if (IPAddress.TryParse(NewValue, out Test))
                            {
                                Console.WriteLine("Set {0} as new CMDB IP Address? Y/N", NewValue);
                                inp = GetYesNo();
                                if (inp == "Y")
                                {
                                    CMDatabaseValues.server = NewValue;
                                    FileAccess.Log(String.Format("Changed CMDB IP to {0}", NewValue));
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid IP Address syntax.");
                            }
                        }
                        else if (inp == "2")
                        {
                            Console.WriteLine("Current Server Port:\t\t{0}", CMDatabaseValues.port);
                            Console.WriteLine("Enter a new Port:");
                            NewValue = GetInput();
                            Console.WriteLine("Set {0} as new CMDB Port? Y/N", NewValue);
                            inp = GetYesNo();
                            if (inp == "Y")
                            {
                                CMDatabaseValues.port = NewValue;
                                FileAccess.Log(String.Format("Changed CMDB Port to {0}", NewValue));
                            }
                        }
                        else if (inp == "3")
                        {
                            Console.WriteLine("Current Server Username:\t{0}", CMDatabaseValues.usrnm);
                            Console.WriteLine("Enter a new Username:");
                            NewValue = GetInput();
                            Console.WriteLine("Set {0} as new CMDB Username? Y/N", NewValue);
                            inp = GetYesNo();
                            if (inp == "Y")
                            {
                                CMDatabaseValues.usrnm = NewValue;
                                FileAccess.Log(String.Format("Changed CMDB Username to {0}", NewValue));
                            }
                        }
                        else if (inp == "4")
                        {
                            Console.WriteLine("Current Server Password:\t{0}", CMDatabaseValues.pwd);
                            Console.WriteLine("Enter a new Password:");
                            NewValue = GetInput();
                            Console.WriteLine("Set {0} as new CMDB Password? Y/N", NewValue);
                            inp = GetYesNo();
                            if (inp == "Y")
                            {
                                CMDatabaseValues.pwd = NewValue;
                                FileAccess.Log(String.Format("Changed CMDB Password to {0}", NewValue));
                            }
                        }
                        else if (inp == "5")
                        {
                            Console.WriteLine("Current Server Default Table:\t{0}", CMDatabaseValues.table);
                            Console.WriteLine("Enter a new Default Table:");
                            NewValue = GetInput();
                            Console.WriteLine("Set {0} as new CMDB Default Table? Y/N", NewValue);
                            inp = GetYesNo();
                            if (inp == "Y")
                            {
                                CMDatabaseValues.table = NewValue;
                                FileAccess.Log(String.Format("Changed CMDB Default Table to {0}", NewValue));
                            }
                        }
                        else if (inp == "0")
                        {
                            inp = null;
                        }
                        Console.WriteLine();
                    }
                    inp = "";
                }
                else if (inp == "2")
                {
                    int LogIterator = 0;
                    String[] LogNames = FileAccess.GetFiles("logs");
                    foreach (String s in LogNames)
                    {
                        Console.WriteLine("[{0}]\t- {1}", LogIterator, s);
                        LogIterator++;
                    }

                    Console.WriteLine("Select a log to view:");
                    inp = LogNames[GetIntBetween(LogNames.Length - 1, 0)];

                    String FileContents = FileAccess.GetLog(inp);
                    if (FileContents != null)
                    {
                        Console.WriteLine("\n==================================================");
                        Console.WriteLine(FileContents);
                        Console.WriteLine("==================================================\n");
                    }
                }
                else if (inp == "3")
                {
                    Console.WriteLine("Install Directory Information\nThe OSHT TMS defaults all logs to be put under (.exe directory)/logs and (.exe directory)/invoices.\n");
                    Console.WriteLine("Current /log & /invoice folder directory:\n{0}", FileAccess.GetInstallPath());

                    Console.WriteLine("Change file output path? Y/N");
                    inp = GetYesNo();

                    if (inp == "Y")
                    {
                        Console.WriteLine("Enter a preexisting directory to redirect files to:");
                        inp = GetInput();
                        if (FileAccess.SetInstallPath(inp))
                        {
                            Console.WriteLine("Successfully set file path to:\n{0}", FileAccess.GetInstallPath());
                        }
                        else
                        {
                            Console.WriteLine("An error occured - the directory inserted does not exist or could not be found.\n" +
                                              "Check logs for more information.");
                        }
                    }
                }
                else if (inp == "4")
                {

                }
                else if (inp == "5")
                {

                }
                else if (inp == "6")
                {
                    Order[] orders = this.tms.GetAllOrders();
                    Console.WriteLine("Enter the directory to dump the TMS backup:");
                    inp = GetInput();
                    if (FileAccess.WriteOrderBackup(orders, inp))
                    {
                        Console.WriteLine("Database backup successfully written to {0}", inp);
                        FileAccess.Log(String.Format("Database backup created at {0}\nWritten to:{1}\t", System.DateTime.Now, inp));
                    }
                    else
                    {
                        Console.WriteLine("Failed to write to DB backup to {0}", inp);
                        FileAccess.Log(String.Format("Failed to generate database backup at {0}\nWritten to:{1}\t", System.DateTime.Now, inp));
                    }
                }
                else if (inp == "0")
                {
                    inp = null;
                }
                Console.WriteLine();
            }
            FileAccess.Log("Administrator logout.");
        }
    }
}
