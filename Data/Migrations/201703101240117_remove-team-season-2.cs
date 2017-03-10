namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeteamseason2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Team", "Season_Id", "dbo.Season");
            DropIndex("dbo.Team", new[] { "Season_Id" });
            DropColumn("dbo.Team", "Season_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Team", "Season_Id", c => c.Guid());
            CreateIndex("dbo.Team", "Season_Id");
            AddForeignKey("dbo.Team", "Season_Id", "dbo.Season", "Id");
        }
    }
}
