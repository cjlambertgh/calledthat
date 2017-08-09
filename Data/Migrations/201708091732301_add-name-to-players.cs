namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnametoplayers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "Name");
        }
    }
}
