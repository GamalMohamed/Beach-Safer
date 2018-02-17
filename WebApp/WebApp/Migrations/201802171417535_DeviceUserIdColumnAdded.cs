namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceUserIdColumnAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "DeviceUserId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "DeviceUserId");
        }
    }
}
