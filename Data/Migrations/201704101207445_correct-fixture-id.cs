namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctfixtureid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Result", "FixtureId", "dbo.Fixture");
            DropPrimaryKey("dbo.Fixture");
            AlterColumn("dbo.Fixture", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Fixture", "Id");
            AddForeignKey("dbo.Result", "FixtureId", "dbo.Fixture", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Result", "FixtureId", "dbo.Fixture");
            DropPrimaryKey("dbo.Fixture");
            AlterColumn("dbo.Fixture", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Fixture", "Id");
            AddForeignKey("dbo.Result", "FixtureId", "dbo.Fixture", "Id", cascadeDelete: true);
        }
    }
}
