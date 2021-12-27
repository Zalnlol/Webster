using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsterWebApp.Migrations
{
    public partial class AddCandidateListTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateList",
                columns: table => new
                {
                    CandidateListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateList", x => x.CandidateListId);
                    table.ForeignKey(
                        name: "FK_CandidateList_Account_Email",
                        column: x => x.Email,
                        principalTable: "Account",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateList_Email",
                table: "CandidateList",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateList");
        }
    }
}
