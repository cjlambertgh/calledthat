namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addapileaguetocompetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competition", "LeagueApiLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competition", "LeagueApiLink");
        }
    }
}
