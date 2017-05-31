namespace YummyCures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "ContentBody", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "ContentBody");
        }
    }
}
