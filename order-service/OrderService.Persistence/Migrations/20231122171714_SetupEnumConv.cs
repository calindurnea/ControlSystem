using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Persistence.Migrations;

/// <inheritdoc />
public partial class SetupEnumConv : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "OrderStatus",
            schema: "orders",
            table: "OrderLifecycle",
            type: "text",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<string>(
            name: "MachineStatus",
            schema: "orders",
            table: "OrderLifecycle",
            type: "text",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<string>(
            name: "OrderType",
            schema: "orders",
            table: "Order",
            type: "text",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "OrderStatus",
            schema: "orders",
            table: "OrderLifecycle",
            type: "integer",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<int>(
            name: "MachineStatus",
            schema: "orders",
            table: "OrderLifecycle",
            type: "integer",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<int>(
            name: "OrderType",
            schema: "orders",
            table: "Order",
            type: "integer",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");
    }
}
