using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class passwordrenter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    bio = table.Column<string>(type: "TEXT", nullable: true),
                    favouritelanguage = table.Column<string>(name: "favourite_language", type: "TEXT", nullable: true),
                    age = table.Column<long>(type: "INTEGER", nullable: true),
                    matches = table.Column<string>(type: "TEXT", nullable: true),
                    likes = table.Column<string>(type: "TEXT", nullable: true),
                    country = table.Column<string>(type: "TEXT", nullable: true),
                    city = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    photourl = table.Column<string>(name: "photo_url", type: "TEXT", nullable: true),
                    ReenterPassword = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
