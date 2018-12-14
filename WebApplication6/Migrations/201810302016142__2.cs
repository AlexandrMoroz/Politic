namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PostDescriptions", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.PostDescriptions", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AlterColumn("dbo.PostDescriptions", "Type", c => c.String());
            AlterColumn("dbo.PostDescriptions", "Content", c => c.String());
            AlterColumn("dbo.Cities", "Name", c => c.String());
        }
    }
}
