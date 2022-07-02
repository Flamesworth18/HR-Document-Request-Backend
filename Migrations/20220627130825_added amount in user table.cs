using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Document_Request.Migrations
{
    public partial class addedamountinusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRequests",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRequests",
                table: "Users");
        }
    }
}
