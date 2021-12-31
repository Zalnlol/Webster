using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;
using WebsterWebApp.Data;

namespace WebsterWebApp.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "e376af47-9b4c-4369-bf93-40ce8450a8f9";
        const string ADMIN_ROLE_GUID = "79c11e1c-38c2-4e6b-a375-96261d4d65d5";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null,"Password100!"); //TODO: Hide Password

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetUsers(" +
                "Id, UserName, NormalizedUserName, " +
                "Email, EmailConfirmed, PhoneNumberConfirmed, " +
                "TwoFactorEnabled, LockoutEnabled, AccessFailedCount, " +
                "NormalizedEmail, PasswordHash, SecurityStamp, FirstName)");
            sb.AppendLine("Values(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");
            sb.AppendLine(",'admin@webster.com.vn'");
            sb.AppendLine(",'ADMIN@WEBSTER.COM.VN'");
            sb.AppendLine(",'admin@webster.com.vn'");
            sb.AppendLine(",0 ");
            sb.AppendLine(",0 ");
            sb.AppendLine(",0 ");
            sb.AppendLine(",0 ");
            sb.AppendLine(",0 ");
            sb.AppendLine(",'ADMIN@WEBSTER.COM.VN'");
            sb.AppendLine($",'{passwordHash}' ");
            sb.AppendLine(", '' ");
            sb.AppendLine(", 'Admin' ");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());
            migrationBuilder.Sql($"INSERT INTO AspNetRoles(Id,Name,NormalizedName) VALUES ('{ADMIN_ROLE_GUID}','Admin','ADMIN')");
            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles(UserId,RoleId) VALUES ('{ADMIN_USER_GUID}', '{ADMIN_ROLE_GUID}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId='{ADMIN_USER_GUID}' AND RoleId='{ADMIN_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{ADMIN_USER_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id='{ADMIN_ROLE_GUID}'");
        }
    }
}
