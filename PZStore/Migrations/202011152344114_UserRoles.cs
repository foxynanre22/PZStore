namespace PZStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            AddColumn("dbo.Customers", "RoleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "RoleID");
            AddForeignKey("dbo.Customers", "RoleID", "dbo.Roles", "RoleID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "RoleID", "dbo.Roles");
            DropIndex("dbo.Customers", new[] { "RoleID" });
            DropColumn("dbo.Customers", "RoleID");
            DropTable("dbo.Roles");
        }
    }
}
