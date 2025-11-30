using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSCHEMAEXAM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Exam_RequiredPercentToPass",
                table: "Exams",
                sql: "[RequiredPercentToPass] <= 100 AND [RequiredPercentToPass] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Exam_RequiredPercentToPass",
                table: "Exams");
        }
    }
}
