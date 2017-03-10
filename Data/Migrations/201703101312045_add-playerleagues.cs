namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addplayerleagues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerLeagues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PlayerId = c.Guid(nullable: false),
                        LeagueId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.League", t => t.LeagueId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.LeagueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerLeagues", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.PlayerLeagues", "LeagueId", "dbo.League");
            DropIndex("dbo.PlayerLeagues", new[] { "LeagueId" });
            DropIndex("dbo.PlayerLeagues", new[] { "PlayerId" });
            DropTable("dbo.PlayerLeagues");
        }
    }
}
