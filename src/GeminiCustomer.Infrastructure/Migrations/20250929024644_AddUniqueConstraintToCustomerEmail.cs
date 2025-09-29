using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiCustomer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCustomerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email_Unique",
                table: "Customers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Email_Unique",
                table: "Customers");
        }
    }
}
