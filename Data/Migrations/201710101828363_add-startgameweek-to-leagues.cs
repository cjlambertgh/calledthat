namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstartgameweektoleagues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.League", "GameweekIdScoringStarts", c => c.Guid());
            CreateIndex("dbo.League", "GameweekIdScoringStarts");
            AddForeignKey("dbo.League", "GameweekIdScoringStarts", "dbo.GameWeek", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.League", "GameweekIdScoringStarts", "dbo.GameWeek");
            DropIndex("dbo.League", new[] { "GameweekIdScoringStarts" });
            DropColumn("dbo.League", "GameweekIdScoringStarts");
        }
    }
}
