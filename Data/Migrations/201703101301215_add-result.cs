namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addresult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Result",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FixtureId = c.Guid(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        AwayScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fixture", t => t.FixtureId, cascadeDelete: true)
                .Index(t => t.FixtureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Result", "FixtureId", "dbo.Fixture");
            DropIndex("dbo.Result", new[] { "FixtureId" });
            DropTable("dbo.Result");
        }
    }
}
