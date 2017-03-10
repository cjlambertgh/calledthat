namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addleagueowners : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeagueOwners",
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
            DropForeignKey("dbo.LeagueOwners", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.LeagueOwners", "LeagueId", "dbo.League");
            DropIndex("dbo.LeagueOwners", new[] { "LeagueId" });
            DropIndex("dbo.LeagueOwners", new[] { "PlayerId" });
            DropTable("dbo.LeagueOwners");
        }
    }
}
