namespace MyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectContext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductionLines", "EquipmentContentAsString", c => c.String());
            AddColumn("dbo.ProductionLines", "CapacityContentAsString", c => c.String());
            DropColumn("dbo.ProductionLines", "StringsAsString");
            DropColumn("dbo.ProductionLines", "IntsAsString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductionLines", "IntsAsString", c => c.String());
            AddColumn("dbo.ProductionLines", "StringsAsString", c => c.String());
            DropColumn("dbo.ProductionLines", "CapacityContentAsString");
            DropColumn("dbo.ProductionLines", "EquipmentContentAsString");
        }
    }
}
