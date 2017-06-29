namespace GeeksForLess_testApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Last_name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Nick_name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "Nick_name");
            DropColumn("dbo.AspNetUsers", "Last_name");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
