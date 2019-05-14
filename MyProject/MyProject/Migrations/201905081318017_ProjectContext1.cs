namespace MyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectContext1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductionLines", "DelayContentAsString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductionLines", "DelayContentAsString");
        }
    }
}
