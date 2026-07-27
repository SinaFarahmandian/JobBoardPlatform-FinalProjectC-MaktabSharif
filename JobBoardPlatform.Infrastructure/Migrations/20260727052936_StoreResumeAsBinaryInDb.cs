using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoreResumeAsBinaryInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResumeUrl",
                table: "AspNetUsers",
                newName: "ResumeFileName");

            migrationBuilder.AddColumn<string>(
                name: "ResumeContentType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ResumeData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResumeData",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ResumeFileName",
                table: "AspNetUsers",
                newName: "ResumeUrl");
        }
    }
}
