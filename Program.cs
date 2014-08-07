using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;


namespace FeedReader
{
    static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);      
            try
            {                
                Utility.LogText("Scheduler Start", "");
                List<GNFeeds> finalFeeds = FeedOperations.GetValidNonPubSubFeeds(0);
                int Counter = 0;
                int.TryParse(Utility.GetConfigValue("Counter"), out Counter);
                List<Thread> fmiThread = new List<Thread>();
                if (finalFeeds != null && finalFeeds.Count > 0)
                {
                    decimal Average = finalFeeds.Average(t => t.PollTime);
                    List<GNFeeds> objFeeds1 = finalFeeds.Where(t => t.PollTime <= Average).ToList();
                    if (objFeeds1 != null && objFeeds1.Count > 0)
                    {
                        Thread fmiThread1 = new Thread(() => StartProcess(objFeeds1));
                        fmiThread1.Start();
                        fmiThread.Add(fmiThread1);
                    }
                    Thread.Sleep(1000);
                    List<GNFeeds> objFeeds2 = finalFeeds.Where(t => t.PollTime > Average).ToList();
                    if (objFeeds2 != null && objFeeds2.Count > 0)
                    {
                        Thread fmiThread2 = new Thread(() => StartProcess(objFeeds2));
                        fmiThread2.Start();
                        fmiThread.Add(fmiThread2);
                    }
                    foreach (var fThread in fmiThread)
                        fThread.Join();                               
                }
            }
            catch (Exception ex)
            {
                Utility.LogText("Error", ex.Message + " " + ex.StackTrace);
            }
            Utility.LogText("Scheduler End", "");
        }

        private static void StartProcess(List<GNFeeds> objFeeds)
        {
            foreach (GNFeeds objFeed in objFeeds)
            {
                if (objFeed.URL.ToLower().Contains("api.twitter.com"))
                    RSS.GetTwitterFeedsAsList(objFeed.URL, objFeed.Id, 2);
                else if (objFeed.URL.ToLower().Contains("craigslist.org"))
                    RSS.GetXMLFeedAsList(objFeed.URL, objFeed.Id, 3);
                else if (objFeed.URL.ToLower().Contains("deals.ebay.com") || objFeed.URL.ToLower().Contains("sites.google.com"))
                    RSS.GetXMLFeedAsList(objFeed.URL, objFeed.Id, 4);
                else
                    RSS.GetXMLFeedAsList(objFeed.URL, objFeed.Id, 5);
            }
        }        

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Utility.LogText("Unhandled Error", e.Message + " " + e.StackTrace);
        }        
    }
}
