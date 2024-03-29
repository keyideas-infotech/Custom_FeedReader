﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace FeedReader
{
    public class FeedOperations
    {
        public static List<GNFeeds> GetValidNonPubSubFeeds(int Action)
        {
            List<GNFeeds> feeds = null;
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Action",Action)
            };
            using (DataTable table = new DataTable()) //List of feeds from database.
            {                
                if (table.Rows.Count > 0)
                {
                    feeds = new List<GNFeeds>();
                    int i = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        GNFeeds feed = new GNFeeds();
                        feed.RowNum = ++i;
                        feed.Id = Convert.ToInt32(row["Id"]);
                        feed.URL = Convert.IsDBNull(row["URL"]) ? string.Empty : (string)row["URL"];
                        feed.PollTime = Convert.IsDBNull(row["PollTime"]) ? 0 : Convert.ToDecimal(row["PollTime"]);
                        feeds.Add(feed);
                    }
                }
            }

            return feeds;
        }

        public static List<GNFeeds> GetValidGroupFeeds(string Pattern)
        {
            List<GNFeeds> feeds = null;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@Pattern", Pattern)                
           };
            using (DataTable table = new DataTable()) //List of feeds from database.
            {                
                if (table.Rows.Count > 0)
                {
                    feeds = new List<GNFeeds>();
                    int i = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        GNFeeds feed = new GNFeeds();
                        feed.RowNum = ++i;
                        feed.Id = Convert.ToInt32(row["Id"]);
                        feed.URL = Convert.IsDBNull(row["URL"]) ? string.Empty : (string)row["URL"];
                        feed.PollTime = Convert.IsDBNull(row["PollTime"]) ? 0 : Convert.ToDecimal(row["PollTime"]);
                        feeds.Add(feed);
                    }
                }
            }

            return feeds;
        }

        public static bool SaveUpdateMail(GNFeedItem item, bool IsPubDate, decimal PollTime, int Action)
        {
            //Write Code for Save Update Feeds in db.
            return true;
        }

        public static bool UpdateFeedFailure(int FeedId, string Message)
        {
            //Write Code for update flag in db when feed failed to retrieved.
            return true;
        }

        public static bool EnableDisableFaultyFeeds(int id, int action)
        {
            //Write Code for enable-disable Feeds in db.
            return true;
        }

    }
}
