namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.People", "Family", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.People", "Surname", c => c.String(maxLength: 20));
            AlterColumn("dbo.People", "Foto", c => c.String(nullable: false));
            AlterColumn("dbo.Positions", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Positions", "Name", c => c.String());
            AlterColumn("dbo.People", "Foto", c => c.String());
            AlterColumn("dbo.People", "Surname", c => c.String());
            AlterColumn("dbo.People", "Family", c => c.String());
            AlterColumn("dbo.People", "Name", c => c.String());
        }
    }
}
