//<auto-generate/>
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabitApi.Migrations.Application;

/// <inheritdoc />
public partial class AddEntries : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "github_access_tokens",
            schema: "dev_habit");

        migrationBuilder.RenameColumn(
            name: "last_completed_utc",
            schema: "dev_habit",
            table: "habits",
            newName: "last_completed_at_utc");

        migrationBuilder.RenameColumn(
            name: "create_at_utc",
            schema: "dev_habit",
            table: "habits",
            newName: "created_at_utc");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            schema: "dev_habit",
            table: "habits",
            type: "character varying(100)",
            maxLength: 100,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(500)",
            oldMaxLength: 500);

        migrationBuilder.AddColumn<int>(
            name: "automation_source",
            schema: "dev_habit",
            table: "habits",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "entries",
            schema: "dev_habit",
            columns: table => new
            {
                id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                habit_id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                user_id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                value = table.Column<int>(type: "integer", nullable: false),
                notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                source = table.Column<int>(type: "integer", nullable: false),
                external_id = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                is_archived = table.Column<bool>(type: "boolean", nullable: false),
                date = table.Column<DateOnly>(type: "date", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_entries", x => x.id);
                table.ForeignKey(
                    name: "fk_entries_habits_habit_id",
                    column: x => x.habit_id,
                    principalSchema: "dev_habit",
                    principalTable: "habits",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_entries_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "dev_habit",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "git_hub_access_tokens",
            schema: "dev_habit",
            columns: table => new
            {
                id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                user_id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_git_hub_access_tokens", x => x.id);
                table.ForeignKey(
                    name: "fk_git_hub_access_tokens_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "dev_habit",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_entries_external_id",
            schema: "dev_habit",
            table: "entries",
            column: "external_id",
            unique: true,
            filter: "external_id IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "ix_entries_habit_id",
            schema: "dev_habit",
            table: "entries",
            column: "habit_id");

        migrationBuilder.CreateIndex(
            name: "ix_entries_user_id",
            schema: "dev_habit",
            table: "entries",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_git_hub_access_tokens_user_id",
            schema: "dev_habit",
            table: "git_hub_access_tokens",
            column: "user_id",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "entries",
            schema: "dev_habit");

        migrationBuilder.DropTable(
            name: "git_hub_access_tokens",
            schema: "dev_habit");

        migrationBuilder.DropColumn(
            name: "automation_source",
            schema: "dev_habit",
            table: "habits");

        migrationBuilder.RenameColumn(
            name: "last_completed_at_utc",
            schema: "dev_habit",
            table: "habits",
            newName: "last_completed_utc");

        migrationBuilder.RenameColumn(
            name: "created_at_utc",
            schema: "dev_habit",
            table: "habits",
            newName: "create_at_utc");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            schema: "dev_habit",
            table: "habits",
            type: "character varying(500)",
            maxLength: 500,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(100)",
            oldMaxLength: 100);

        migrationBuilder.CreateTable(
            name: "github_access_tokens",
            schema: "dev_habit",
            columns: table => new
            {
                id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                create_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                user_id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_github_access_tokens", x => x.id);
                table.ForeignKey(
                    name: "fk_github_access_tokens_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "dev_habit",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_github_access_tokens_user_id",
            schema: "dev_habit",
            table: "github_access_tokens",
            column: "user_id",
            unique: true);
    }
}
