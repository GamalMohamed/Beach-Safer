namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedStatusToState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceLogs", "State", c => c.String());
            DropColumn("dbo.DeviceLogs", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceLogs", "Status", c => c.String());
            DropColumn("dbo.DeviceLogs", "State");
        }
    }
}
