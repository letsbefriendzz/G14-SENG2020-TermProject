﻿/*
 * FILE             : FileAccess.cs
 * PROJECT          : SENG2020 - Term Project
 * PROGRAMMER(s)    : Ryan Enns
 * FIRST VERSION    : 2021-11-25
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
            if (o.mr.VanType == 0)
                VanType = "Regular";
            else VanType = "Reefer";


            String filename = String.Format("{0}-{1}-{2}-{3}.txt", o.GetID(), o.mr.ClientName, o.GetOrigin(), o.GetDestin());
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
                                           o.GetID(), o.GetClient(), o.JobType(), o.GetOrigin(), o.GetDestin(), VanType, o.GetTimeToComplete(), o.GetDistanceToComplete(), o.GetCostToComplete(), o.GetOSHTSurcharge(), o.IsComplete);
            
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

        public static String[] GetLogs()
        {
            String[] ReturnValues = null;

            ReturnValues = Directory.GetFiles(InstallPath + "/logs");

            for (int i = 0; i < ReturnValues.Length; i++)
                ReturnValues[i] = Path.GetFileName(ReturnValues[i]);

            return ReturnValues;
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
