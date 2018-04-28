namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastSeaPointAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beaches", "LastSeaPoint", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Beaches", "LastSeaPoint");
        }
    }
}
