namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBeachProps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beaches", "StartPoint", c => c.String());
            AddColumn("dbo.Beaches", "EndPoint", c => c.String());
            DropColumn("dbo.Beaches", "LongitudeBegin");
            DropColumn("dbo.Beaches", "LongitudeEnd");
            DropColumn("dbo.Beaches", "Latitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Beaches", "Latitude", c => c.String());
            AddColumn("dbo.Beaches", "LongitudeEnd", c => c.String());
            AddColumn("dbo.Beaches", "LongitudeBegin", c => c.String());
            DropColumn("dbo.Beaches", "EndPoint");
            DropColumn("dbo.Beaches", "StartPoint");
        }
    }
}
