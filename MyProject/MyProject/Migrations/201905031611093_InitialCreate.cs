namespace MyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentId = c.Guid(nullable: false),
                        DateOfCreation = c.DateTime(nullable: false),
                        Name = c.String(),
                        Producer = c.String(),
                        Type = c.String(),
                        Productivity = c.Int(nullable: false),
                        Characteristics = c.String(),
                        imageData = c.Binary(),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
            CreateTable(
                "dbo.ProductionLines",
                c => new
                    {
                        ProductionLineId = c.Guid(nullable: false),
                        DateOfCreation = c.DateTime(nullable: false),
                        Name = c.String(),
                        User = c.String(),
                        StringsAsString = c.String(),
                        IntsAsString = c.String(),
                    })
                .PrimaryKey(t => t.ProductionLineId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductionLines");
            DropTable("dbo.Equipments");
        }
    }
}
