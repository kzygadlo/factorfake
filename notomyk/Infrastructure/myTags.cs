using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public static class myTags
    {
        public static void AddTags(int newsID, string keywords)
        {
            if (keywords.Length > 0)
            {
                char separatorChar = ';';
                if (keywords.ToLower().IndexOf(';') == -1)
                {
                    separatorChar = ',';
                }

                ICollection<string> ListOfTags = keywords.Split(separatorChar).ToList();

                if (ListOfTags.Count > 0)
                {
                    AddTagsToDB(newsID, ListOfTags);
                }
            }
        }

        private static void AddTagsToDB(int newsID, ICollection<string> Tags)
        {
            using (NTMContext db = new NTMContext())
            {
                int tagID;
                Tag singleTag = new Tag();

                RemoveTagReferenceFromNews(newsID);

                foreach (var tag in Tags)
                {
                    var tagTrimed = tag.Trim();
                    if (db.Tag.Any(t => t.TagName == tagTrimed))
                    {
                        var tagFromDB = db.Tag.Where(t => t.TagName == tagTrimed).FirstOrDefault();
                        AddTagReferenceToNews(newsID, tagFromDB.ID);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tagTrimed))
                        {
                            singleTag.TagName = tagTrimed;
                            db.Tag.Add(singleTag);
                            db.SaveChanges();

                            tagID = singleTag.ID;
                            AddTagReferenceToNews(newsID, tagID);
                        }

                    }
                }
                db.SaveChanges();
            }
        }

        private static void AddTagReferenceToNews(int newsID, int tagID)
        {
            using (NTMContext db = new NTMContext())
            {
                if (!db.EventTag.Any(e => e.tbl_NewsID == newsID && e.TagID == tagID))
                {
                    EventTag eventT = new EventTag();
                    eventT.TagID = tagID;
                    eventT.tbl_NewsID = newsID;
                    db.EventTag.Add(eventT);
                    db.SaveChanges();
                }
            }
        }

        private static void RemoveTagReferenceFromNews(int newsID)
        {
            using (NTMContext db = new NTMContext())
            {
                if (db.EventTag.Any(e => e.tbl_NewsID == newsID))
                {
                    IEnumerable<EventTag> tagsToRemove = (from t in db.EventTag
                                                          where t.tbl_NewsID == newsID
                                                          select t).ToList();

                    foreach (EventTag tag in tagsToRemove)
                    {
                        var sTag = db.Tag.FirstOrDefault(t => t.ID == tag.TagID);

                        db.EventTag.Remove(tag);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static string TagsBuilder(ICollection<string> listOfTags)
        {
            string joinedTags = string.Join(",", listOfTags);

            return joinedTags;
        }

        public static string TagsStringForListOfNews(ICollection<string> listOfTags)
        {
            int counter = 0;
            string result = string.Empty;

            foreach (var tag in listOfTags)
            {
                if (result.Length > 0)
                {
                    result = result + " | ";
                }
                result = result + tag;
            }
            return result;
        }
    }
}