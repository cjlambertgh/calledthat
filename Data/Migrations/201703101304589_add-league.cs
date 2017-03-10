namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addleague : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.League",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompetitionId = c.Guid(nullable: false),
                        Name = c.String(),
                        InviteCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competition", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.League", "CompetitionId", "dbo.Competition");
            DropIndex("dbo.League", new[] { "CompetitionId" });
            DropTable("dbo.League");
        }
    }
}
