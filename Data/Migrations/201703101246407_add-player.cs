namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addplayer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TotalScore = c.Int(nullable: false),
                        GameWeekScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Player", "UserId", "dbo.AppUser");
            DropIndex("dbo.Player", new[] { "UserId" });
            DropTable("dbo.Player");
        }
    }
}
