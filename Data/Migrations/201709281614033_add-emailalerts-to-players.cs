namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addemailalertstoplayers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "EmailAlerts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "EmailAlerts");
        }
    }
}
