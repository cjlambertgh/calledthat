namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeteamseason : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Team", "SeasonId", "dbo.Season");
            DropIndex("dbo.Team", new[] { "SeasonId" });
            RenameColumn(table: "dbo.Team", name: "SeasonId", newName: "Season_Id");
            AlterColumn("dbo.Team", "Season_Id", c => c.Guid());
            CreateIndex("dbo.Team", "Season_Id");
            AddForeignKey("dbo.Team", "Season_Id", "dbo.Season", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "Season_Id", "dbo.Season");
            DropIndex("dbo.Team", new[] { "Season_Id" });
            AlterColumn("dbo.Team", "Season_Id", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Team", name: "Season_Id", newName: "SeasonId");
            CreateIndex("dbo.Team", "SeasonId");
            AddForeignKey("dbo.Team", "SeasonId", "dbo.Season", "Id", cascadeDelete: true);
        }
    }
}
