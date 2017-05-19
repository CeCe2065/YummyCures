namespace YummyCures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailUrlproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "ThumbNailUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "ThumbNailUrl");
        }
    }
}
