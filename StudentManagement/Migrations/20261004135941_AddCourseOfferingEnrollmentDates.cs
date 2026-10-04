using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseOfferingEnrollmentDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentEndDate",
                table: "CourseOfferings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentStartDate",
                table: "CourseOfferings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddCheckConstraint(
                name: "CK_CourseOffering_EnrollmentEndDate_After_StartDate",
                table: "CourseOfferings",
                sql: "[EnrollmentEndDate] >= [EnrollmentStartDate]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CourseOffering_EnrollmentStartDate_Before_StartDate",
                table: "CourseOfferings",
                sql: "[EnrollmentStartDate] <= [StartDate]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CourseOffering_EnrollmentEndDate_After_StartDate",
                table: "CourseOfferings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CourseOffering_EnrollmentStartDate_Before_StartDate",
                table: "CourseOfferings");

            migrationBuilder.DropColumn(
                name: "EnrollmentEndDate",
                table: "CourseOfferings");

            migrationBuilder.DropColumn(
                name: "EnrollmentStartDate",
                table: "CourseOfferings");
        }
    }
}
