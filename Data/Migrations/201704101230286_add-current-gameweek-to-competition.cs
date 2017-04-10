namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcurrentgameweektocompetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competition", "CurrentGameWeekNumber", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competition", "CurrentGameWeekNumber");
        }
    }
}
