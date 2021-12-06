/*
 * FILE             : FileAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-12-03
 * DESCRIPTION      :
 *  This file defines all File IO related methods. The static FileAccess class is intended to allow
 *  client code to create logs in a given directory, let the Buyer create invoices in new text files,
 *  get present log files, and read from log files.
 */

using System;
using System.IO;
using System.Reflection;
using SENG2020_TermProject.Data_Logic;

namespace SENG2020_TermProject.Communications
{
    static class FileAccess
    {
        //default install path - the directory that the exe is being executed from
        private static String InstallPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /*
         * NAME : SetInstallPath
         * DESC :
         *  The SetInstallPath function accepts a new pathway from an administrator
         *  to set the InstallDirectory. If the directory exists, the InstalPath field
         *  is updated and the /logs and /invoices folders are created as subdirectories.
         *  
         *  In each case, a Log is created to track the attempted change in system.
         *  
         *  If the function is successful, true is returned. Otherwise, false is returned.
         * RTRN : bool
         * PARM : String
         */
        public static bool SetInstallPath(String path)
        {
            if (Directory.Exists(path))
            {
               
                InstallPath = path;
                initInstallDirectories();
                Log(String.Format("File dump directory changed from:\n{0}\n\nto\n\n{1}", InstallPath, path));
                return true;
            }
            else
            {
                Log(String.Format("Error in install path change attempt;\n{0}\nIs not a working directory on this machine.", path));
                return false;
            }
        }

        public static bool WriteOrderBackup(Order[] orders, String pathway)
        {
            if (!Directory.Exists(pathway)) return false;

            String output = "ID,ClientName,JobType,Origin,Destination,VanType,TimeToComplete,DistanceToComplete,CostToComplete,OSHTSurcharge,IsComplete\n";
            foreach (Order o in orders)
            {
                //id-client-jobtype-origin-destination-vantype-time-distance-cost-oshtcharge-iscomplete
                if (o.GetIsComplete())
                {
                    output += String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},",
                                            o.GetID(), o.GetClient(), o.GetJobType(), o.GetOrigin(), o.GetDestin(), o.GetVanType(), o.GetTimeToComplete(), o.GetDistanceToComplete(), o.GetCostToComplete(), o.GetOSHTSurcharge(), o.GetIsComplete());
                }
                else
                {

                    output += String.Format("{0},{1},{2},{3},{4},{5},null,null,null,null,{6}",
                                            o.GetID(), o.GetClient(), o.GetJobType(), o.GetOrigin(), o.GetDestin(), o.GetVanType(), o.GetIsComplete());
                }
                output += "\n";
            }

            pathway = pathway + "/" + String.Format("{0}-OrderDBBackup.txt", System.DateTime.Now.ToString().Replace(":", "-").Replace(" ", "_"));
            File.Create(pathway).Close();
            File.WriteAllText(pathway, output);
            return true;
        }

        //a simple getter for the static String InstallPath
        public static String GetInstallPath()
        {
            return InstallPath;
        }
        
        /*
         * NAME : CreateInvoice
         * DESC :
         *  This function creates an invoice file and writes information from an Order object to it. This is
         *  achieved through use of System.IO's File.Create and File.WriteAllText methods.
         *  
         *  This method returns the pathway to the file that it just created.
         * RTRN : String
         * PARM : Order
         */
        public static String CreateInvoice(Order o)
        {
            String VanType;
            if (o.mr.GetVanType() == 0)
                VanType = "Regular";
            else VanType = "Reefer";


            String filename = String.Format("{0}-{1}-{2}-{3}.txt", o.GetID(), o.mr.GetClientName(), o.GetOrigin(), o.GetDestin());
            String filepath = InstallPath + "/invoices/" + filename;
            String invoice = String.Format("OrderID:\t\t{0}\n" +
                                           "ClientName:\t\t{1}\n" +
                                           "JobType:\t\t{2}\n" +
                                           "Origin:\t\t\t{3}\n" +
                                           "Destination:\t{4}\n" +
                                           "Van Type:\t\t{5}\n\n" +
                                           "Elapsed Time:\t\t{6}\n" +
                                           "Distance:\t\t\t{7}\n" +
                                           "Carrier Charges:\t{8}\n" +
                                           "OSHT Charges:\t\t{9}\n" +
                                           "Status:\t\t\t\t{10}\n",
                                           o.GetID(), o.GetClient(), o.JobType(), o.GetOrigin(), o.GetDestin(), VanType, o.GetTimeToComplete(), o.GetDistanceToComplete(), o.GetCostToComplete(), o.GetOSHTSurcharge(), o.GetIsComplete());
            
            File.Create(filepath).Close(); //close fstream
            File.WriteAllText(filepath, invoice);

            return filepath;
        }


        /*
         * NAME : initInstallDirectory()
         * DESC :
         *  To be ran at the start of the main thread of execution, the initInstallDirectory
         *  function ensures that the logs and invoices subdirectories exist in the directory
         *  of execution of the TMS application. If they do not exist, they're created.
         * RTRN : //
         * PARM : //
         */
        public static void initInstallDirectories()
        {
            if (!Directory.Exists(InstallPath + "/logs"))
                Directory.CreateDirectory(InstallPath + "/logs");

            if (!Directory.Exists(InstallPath + "/invoices"))
                Directory.CreateDirectory(InstallPath + "/invoices");
        }

        /*
         * NAME : GetLogs
         * DESC :
         *  This function gets all the filenames that are present in the specified installpath/logs
         *  folder. It returns an array of Strings that contains each filename present.
         * RTRN : String[]
         * PARM : //
         */
        public static String[] GetLogs()
        {
            String[] ReturnValues = null;

            ReturnValues = Directory.GetFiles(InstallPath + "/logs");

            for (int i = 0; i < ReturnValues.Length; i++)
                ReturnValues[i] = Path.GetFileName(ReturnValues[i]);

            return ReturnValues;
        }

        public static String GetLog(String name)
        {
            String path = InstallPath + "/logs/" + name;
            if (File.Exists(path))
                return File.ReadAllText(path);
            else return null;
        }

        /*
         * NAME : Log
         * DESC :
         *  This function creates a log file out of the System.DateTime.Now object and a message passed to it.
         *  It creates a new file, constructed from the date and time of the log creation. A file is created
         *  using System.IO's File.Create method and written to using File.WriteAllText.
         *  
         *  True is returned if the file writing was successful. If there is a System.IO error, the exception
         *  thrown is dumped to the console as a last line of defence; can't log this error when the error is
         *  in the logging function ! :)
         * RTRN : bool
         * PARM : String
         */
        public static bool Log(String message)
        {
            String log = String.Format("Current Time:\n{0}\nError Message:\n{1}", System.DateTime.Now, message);
            String filename = System.DateTime.Now.ToString().Replace(":", "-").Replace(" ", "_");
            String filepath = InstallPath + "/logs/" + filename + ".log";

            try
            {
                File.Create(filepath).Close();
                File.WriteAllText(filepath, log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}
