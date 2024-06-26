namespace RTCodingExercise.Monolithic.Migrations;

public partial class Reserve : Migration {
    
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Boolean>("Reserved", "plates", "bit", defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("Reserved", "plates");
    }
}