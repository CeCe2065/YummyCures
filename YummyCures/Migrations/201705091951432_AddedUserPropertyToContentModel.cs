namespace YummyCures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserPropertyToContentModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contents", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Contents", "UserID");
            AddForeignKey("dbo.Contents", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contents", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Contents", new[] { "UserID" });
            AlterColumn("dbo.Contents", "UserID", c => c.String());
        }
    }
}
