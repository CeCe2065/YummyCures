namespace YummyCures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SearchViewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Description");
        }
    }
}
