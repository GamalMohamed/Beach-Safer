namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedSubscriptionToEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Subscription", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "SubscriptionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "SubscriptionType", c => c.String());
            DropColumn("dbo.Customers", "Subscription");
        }
    }
}
