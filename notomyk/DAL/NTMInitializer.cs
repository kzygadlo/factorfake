using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace notomyk.DAL
{
    //public class NTMInitializer : DropCreateDatabaseIfModelChanges<NTMContext>
    public class NTMInitializer : DropCreateDatabaseAlways<NTMContext>
    {
        protected override void Seed(NTMContext context)
        {
            SeedStoreData(context);
            base.Seed(context);
        }

        private void SeedStoreData(NTMContext context)
        {
            

            //var Newspapes = new List<tbl_Newspaper>
            //{
            //    new tbl_Newspaper() { tbl_NewspaperID = 1, NewspaperLink="", NewspaperIconLink="wyborcza.ico", NewspaperName="Wyborcza" },
            //    new tbl_Newspaper() { tbl_NewspaperID = 2, NewspaperLink="", NewspaperIconLink="newsweek.ico", NewspaperName="Newsweek" },
            //    new tbl_Newspaper() { tbl_NewspaperID = 3, NewspaperLink="", NewspaperIconLink="niezalezna.jpg", NewspaperName="Niezalezna" }
            //};

            //Newspapes.ForEach(n => context.Newspaper.Add(n));
            //context.SaveChanges();

            //var News = new List<tbl_News>
            //{
            //    new tbl_News() { tbl_NewsID=1, tbl_NewspaperID = 1, tbl_UserID = 1, Link = "link1", IsActive=true, Title="Title1", Fakt = 1, Fake =2, DateAdd = DateTime.Now },
            //    new tbl_News() { tbl_NewsID=2, tbl_NewspaperID = 2, tbl_UserID = 2, Link = "link2", IsActive=true, Title="Title2", Fakt = 3, Fake =2, DateAdd = DateTime.Now },
            //    new tbl_News() { tbl_NewsID=3, tbl_NewspaperID = 3, tbl_UserID = 2, Link = "link3", IsActive=true, Title="Title3", Fakt = 4, Fake =2, DateAdd = DateTime.Now }
            //};

            //News.ForEach(n => context.News.Add(n));
            //context.SaveChanges();

            //var Comments = new List<tbl_Comment>
            //{
            //    new tbl_Comment() { DateAdd=DateTime.Now, tbl_CommentID=1,tbl_NewsID=1, Fake=17, Fakt=2, Visible=true,  Comment="Sapien elit in malesuada semper mi, id sollicitudin urna fermentum."},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=2,tbl_NewsID=1, Fake=1, Fakt=10, Visible=true , Comment="In malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl." },
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=3,tbl_NewsID=1, Fake=51, Fakt=3, Visible=true , Comment="Semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel."},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=4,tbl_NewsID=1, Fake=1, Fakt=7, Visible=true , Comment="Vel sapien elit in malesuada semper mi, id sollicitudin." },

            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=5,tbl_NewsID=2, Fake=41, Fakt=8, Visible=true ,Comment="Mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus."},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=6,tbl_NewsID=2, Fake=1, Fakt=5, Visible=true , Comment="Adipiscing elit fusce vel sapien elit."},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=7,tbl_NewsID=2, Fake=11, Fakt=10, Visible=true , Comment="Sapien elit in malesuada semper mi, id sollicitudin urna fermentum." },

            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=8,tbl_NewsID=3, Fake=31, Fakt=1, Visible=true , Comment="Kom10"},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=9,tbl_NewsID=3, Fake=1, Fakt=10, Visible=true,  Comment="Consectetur adipiscing elit fusce vel."},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=10,tbl_NewsID=3, Fake=21, Fakt=1, Visible=true , Comment="Bla bla bla" },
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=11,tbl_NewsID=3, Fake=1, Fakt=0, Visible=true , Comment="xxx"},
            //    new tbl_Comment() { DateAdd=DateTime.Now,tbl_CommentID=12,tbl_NewsID=3, Fake=11, Fakt=9, Visible=true , Comment="Sapien elit in malesuada semper mi, id sollicitudin urna fermentum.Elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce." }

            //};

            //Comments.ForEach(c => context.Comment.Add(c));
            //context.SaveChanges();

        }
    }
}









