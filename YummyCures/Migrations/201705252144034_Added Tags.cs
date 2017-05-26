namespace YummyCures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.TagContents",
                c => new
                    {
                        Tag_TagID = c.Int(nullable: false),
                        Content_ContentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagID, t.Content_ContentID })
                .ForeignKey("dbo.Tags", t => t.Tag_TagID, cascadeDelete: true)
                .ForeignKey("dbo.Contents", t => t.Content_ContentID, cascadeDelete: true)
                .Index(t => t.Tag_TagID)
                .Index(t => t.Content_ContentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagContents", "Content_ContentID", "dbo.Contents");
            DropForeignKey("dbo.TagContents", "Tag_TagID", "dbo.Tags");
            DropIndex("dbo.TagContents", new[] { "Content_ContentID" });
            DropIndex("dbo.TagContents", new[] { "Tag_TagID" });
            DropTable("dbo.TagContents");
            DropTable("dbo.Tags");
        }
    }
}
