using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "邮件地址",
                table: "Customers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "是否发送邮件",
                table: "Customers",
                newName: "WelcomeEmailWasSent");

            migrationBuilder.RenameColumn(
                name: "姓名",
                table: "Customers",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "longtext",
                nullable: false,
                comment: "邮箱",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "WelcomeEmailWasSent",
                table: "Customers",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否发送过欢迎邮件",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "longtext",
                nullable: false,
                comment: "姓名",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WelcomeEmailWasSent",
                table: "Customers",
                newName: "是否发送邮件");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customers",
                newName: "姓名");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customers",
                newName: "邮件地址");

            migrationBuilder.AlterColumn<bool>(
                name: "是否发送邮件",
                table: "Customers",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否发送过欢迎邮件");

            migrationBuilder.AlterColumn<string>(
                name: "姓名",
                table: "Customers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "姓名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "邮件地址",
                table: "Customers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "邮箱")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
