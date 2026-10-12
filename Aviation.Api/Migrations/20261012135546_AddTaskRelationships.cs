using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviation.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PriorityId",
                table: "MaintenanceTasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "MaintenanceTasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(
                """
                IF EXISTS
                (
                    SELECT 1
                    FROM [dbo].[MaintenanceTasks] AS task
                    OUTER APPLY
                    (
                        SELECT COUNT_BIG(*) AS [MatchCount]
                        FROM [dbo].[Statuses] AS status
                        WHERE status.[Name] = task.[Status]
                    ) AS statusMatch
                    OUTER APPLY
                    (
                        SELECT COUNT_BIG(*) AS [MatchCount]
                        FROM [dbo].[Priorities] AS priority
                        WHERE priority.[Name] = task.[Priority]
                    ) AS priorityMatch
                    WHERE statusMatch.[MatchCount] <> 1
                       OR priorityMatch.[MatchCount] <> 1
                )
                BEGIN
                    THROW 51000, 'Cannot migrate MaintenanceTasks: each legacy Status and Priority value must match exactly one lookup row.', 1;
                END;

                UPDATE task
                SET [StatusId] = status.[Id],
                    [PriorityId] = priority.[Id]
                FROM [dbo].[MaintenanceTasks] AS task
                INNER JOIN [dbo].[Statuses] AS status
                    ON status.[Name] = task.[Status]
                INNER JOIN [dbo].[Priorities] AS priority
                    ON priority.[Name] = task.[Priority];
                """);

            migrationBuilder.AlterColumn<long>(
                name: "StatusId",
                table: "MaintenanceTasks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PriorityId",
                table: "MaintenanceTasks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "MaintenanceTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MaintenanceTasks");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTasks_PriorityId",
                table: "MaintenanceTasks",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTasks_StatusId",
                table: "MaintenanceTasks",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTasks_Priorities_PriorityId",
                table: "MaintenanceTasks",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceTasks_Statuses_StatusId",
                table: "MaintenanceTasks",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTasks_Priorities_PriorityId",
                table: "MaintenanceTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceTasks_Statuses_StatusId",
                table: "MaintenanceTasks");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTasks_PriorityId",
                table: "MaintenanceTasks");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceTasks_StatusId",
                table: "MaintenanceTasks");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "MaintenanceTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MaintenanceTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE task
                SET [Status] = status.[Name],
                    [Priority] = priority.[Name]
                FROM [dbo].[MaintenanceTasks] AS task
                INNER JOIN [dbo].[Statuses] AS status
                    ON status.[Id] = task.[StatusId]
                INNER JOIN [dbo].[Priorities] AS priority
                    ON priority.[Id] = task.[PriorityId];
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "MaintenanceTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaintenanceTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "MaintenanceTasks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "MaintenanceTasks");
        }
    }
}
