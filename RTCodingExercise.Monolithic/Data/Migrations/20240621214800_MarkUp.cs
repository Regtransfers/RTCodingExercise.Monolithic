namespace RTCodingExercise.Monolithic.Migrations;

public partial class MarkUp : Migration {

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"ALTER TABLE Plates ADD MarkUp AS (SalePrice * 1.2) PERSISTED");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"ALTER TABLE DROP COLUMN MarkUp");
    }
}