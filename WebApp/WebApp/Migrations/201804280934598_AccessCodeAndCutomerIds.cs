namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessCodeAndCutomerIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AccessCode", c => c.String());
            AddColumn("dbo.Customers", "AccessCodeHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "AccessCodeHash");
            DropColumn("dbo.Customers", "AccessCode");
        }
    }
}
