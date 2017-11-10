namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            string Sql1 = @"
insert into tbl_Newspaper (NewspaperName,NewspaperLink, NewspaperIconLink)
values
('www.wyborcza.pl','www.wyborcza.pl','logo_wyborcza.jpg'),
('www.newsweek.pl','www.newsweek.pl','logo_newsweek.jpg'),
('www.onet.pl','www.onet.pl','logo_onet.jpg'),
('www.tvn.pl','www.tvn.pl','logo_tvn.jpg'),
('www.wp.pl','www.wp.pl','logo_wp.jpg'),
('www.wprost.pl','www.wprost.pl','logo_wprost.jpg'),
('www.gazetawroclawska.pl','www.gazetawroclawska.pl','logo_gwroclawska.jpg'),
('www.wpolityce.pl','www.wpolityce.pl','logo_wpolityce.jpg'),
('www.fakt.pl','www.fakt.pl','logo_fakt.jpg'),
('www.sport.interia.pl','www.sport.interia.pl','logo_interia.jpg'),
('www.tvn24.pl','www.tvn24.pl','logo_tvn24.jpg'),
('www.niezalezna.pl','www.niezalezna.pl','logo_niezalezna.jpg'),
('www.m.se.pl','www.m.se.pl','logo_se.jpg'),
('www.wgospodarce.pl','www.wgospodarce.pl','logo_wgospodarce.jpg'),
('www.facet.interia.pl','www.facet.interia.pl','logo_interiafacet.jpg'),
('www.kobieta.interia.pl','www.kobieta.interia.pl','logo_interiakobieta.jpg')";

            Sql(Sql1);
        }
        
        public override void Down()
        {

        }
    }
}
