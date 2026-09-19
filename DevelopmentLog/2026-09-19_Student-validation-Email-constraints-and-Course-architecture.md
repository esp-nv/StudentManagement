# Development Log – 2026-09-19

## Student Model and Database

- Continued work on `Student` model and database consistency.
- Replaced persisted `Age` with:
  - `DateOfBirth`
  - `IsDateOfBirthEstimated`
- Added migration `AddDateOfBirthAndEstimatedFlag`.
- Existing `Age` values were used to generate estimated dates of birth.
- Added migration `RemoveAgeFromStudent`.
- Removed `Age` column from `Students`.
- Kept `CalculateAge()` in `StudentService` so age can be calculated when needed.

## FirstName Validation

- Added centralized constants:
  - `MinFirstNameLength = 2`
  - `MaxFirstNameLength = 100`
- Added `StringLength` validation to `FirstName`.
- Updated database column from `nvarchar(max)` to `nvarchar(100)`.
- Applied migration successfully.
- Verified the database schema.

## LastName Validation

- Added centralized constants:
  - `MinLastNameLength = 2`
  - `MaxLastNameLength = 100`
- Added `Required` and `StringLength` validation to `LastName`.
- Updated database column from `nvarchar(max)` to `nvarchar(100)`.
- Created and applied migration `AddNameLengthConstraints`.
- Verified the database schema.

## Email Validation

- Confirmed `Required` and `EmailAddress` validation.
- Added maximum length of `254`.
- Updated database column from `nvarchar(max)` to `nvarchar(254)`.
- Created migration `AddEmailLengthConstraint`.
- Applied migration successfully.
- Verified the database schema.

## Student UI

- Updated Create/Edit views.
- Added `*` indicator for required fields.
- Kept UI validation consistent with model validation.
- Used `StudentConstants` where appropriate.

## Course Architecture Discussion

- Discussed replacing the current free-text `Course` field with a separate `Course` entity.
- Determined that the relationship between `Student` and `Course` should be Many-to-Many.
- Planned an intermediate `StudentCourses` relationship.
- Discussed using multi-select for courses in Student Create/Edit.

### Course – Version 1

Initial Course model planned as:

```text
Course
------
Id
Code
Name
Description
