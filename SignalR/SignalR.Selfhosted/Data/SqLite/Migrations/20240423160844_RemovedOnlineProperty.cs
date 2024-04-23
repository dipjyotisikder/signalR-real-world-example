using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR.SelfHosted.Data.SqLite.Migrations
{
    public partial class RemovedOnlineProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnLine",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnLine",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
