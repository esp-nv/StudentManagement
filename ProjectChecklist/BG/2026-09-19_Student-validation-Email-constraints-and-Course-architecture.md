# Checklist – 2026-09-19

## Student – DateOfBirth

- [x] Добавено `DateOfBirth` в `Student`
- [x] Добавено `IsDateOfBirthEstimated` в `Student`
- [x] Добавена migration `AddDateOfBirthAndEstimatedFlag`
- [x] Съществуващите `Age` стойности са използвани за приблизително изчисляване на `DateOfBirth`
- [x] Старите записи са маркирани с `IsDateOfBirthEstimated = true`
- [x] Migration-ът е приложен успешно
- [x] Проверена е базата

## Student – Age

- [x] `Age` е премахнат от `Student`
- [x] `CalculateAge()` остава в `StudentService`
- [x] Добавена migration `RemoveAgeFromStudent`
- [x] Migration-ът е приложен успешно
- [x] Колоната `Age` е премахната от `Students`
- [x] Build минава успешно

## FirstName

- [x] Добавен `MinFirstNameLength = 2`
- [x] Добавен `MaxFirstNameLength = 100`
- [x] Добавен `[Required]`
- [x] Добавен `[StringLength(...)]`
- [x] Database колоната е променена на `nvarchar(100)`
- [x] Проверена е базата

## LastName

- [x] Добавен `MinLastNameLength = 2`
- [x] Добавен `MaxLastNameLength = 100`
- [x] Добавен `[Required]`
- [x] Добавен `[StringLength(...)]`
- [x] Database колоната е променена на `nvarchar(100)`
- [x] Създадена migration `AddNameLengthConstraints`
- [x] Migration-ът е приложен успешно
- [x] Проверена е базата

## Email

- [x] Добавен `[Required]`
- [x] Добавен `[EmailAddress]`
- [x] Определена максимална дължина `254`
- [x] Database колоната е променена на `nvarchar(254)`
- [x] Създадена migration `AddEmailLengthConstraint`
- [x] Migration-ът е приложен успешно
- [x] Проверена е базата

## UI – Student

- [x] Добавена `*` индикация за задължителните полета
- [x] Create UI е актуализиран
- [x] Edit UI е актуализиран
- [x] UI validation е съобразена с Model validation
- [x] Използван е `StudentConstants`, когато е необходимо

## Architecture – Courses

### Planning

- [x] Обсъдена е необходимостта от отделна `Courses` таблица
- [x] Решено е `Student` да няма свободен текстов `Course`
- [x] Решена е Many-to-Many връзка `Student ↔ Course`
- [x] Предвидена е междинна таблица `StudentCourses`
- [x] Решено е Course да бъде отделен модел

### Course – Version 1

- [x] Определено е `Course` да съдържа:
  - [x] `Id`
  - [x] `Code`
  - [x] `Name`
  - [x] `Description`

- [x] Решено е `Code` да бъде стабилен код на курса
- [x] `Code` няма да съдържа година
- [x] `Code` няма да съдържа месец
- [x] `Code` няма да съдържа преподавател
- [x] `Code` няма да съдържа версия

### Student ↔ Course

- [x] Един студент може да има много курсове
- [x] Един курс може да има много студенти
- [x] В Student Create/Edit ще се използва multi-select
- [x] Няма да се използва свободен текст за Course

## Future – Version 2+

- [ ] `CourseOffering`
- [ ] Година на провеждане
- [ ] Месец на провеждане
- [ ] Преподавател
- [ ] StartDate / EndDate
- [ ] `CourseVersion`
- [ ] `Topic`
- [ ] Връзка CourseVersion ↔ Topic
- [ ] Enrollment като отделно бизнес понятие
- [ ] EnrollmentDate
- [ ] EnrollmentStatus
- [ ] Grade
- [ ] Manager управление на CourseOfferings

## Architecture

- [x] Запазена Layered Architecture
- [x] `View → Controller → Service → DbContext → SQL Server`
- [x] Няма добавен Repository Pattern
- [x] Няма добавен DTO layer без необходимост
- [x] Архитектурата остава достатъчно проста за текущата версия

## Next Session

- [ ] Създаване на `Course.cs`
- [ ] Определяне на validation правилата за Course
- [ ] Добавяне на `DbSet<Course>`
- [ ] Конфигуриране на Many-to-Many `Student ↔ Course`
- [ ] Определяне на `StudentCourses`
- [ ] Проверка на EF Core model
- [ ] Build
- [ ] Създаване на migration
- [ ] Update-ване на database
- [ ] Създаване на Course Service
- [ ] Създаване на Courses Controller
- [ ] Създаване на Course Views
- [ ] Multi-select в Student Create
- [ ] Multi-select в Student Edit
- [ ] Показване на избраните курсове в Student UI
