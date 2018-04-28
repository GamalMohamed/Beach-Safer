namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmergencyPhoneAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceUsers", "EmergencyPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceUsers", "EmergencyPhone");
        }
    }
}
