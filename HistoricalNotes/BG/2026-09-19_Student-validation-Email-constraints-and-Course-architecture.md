**Historical Note**
## 2026-09-19 – Student validation, Email constraints and Course architecture
## Context

Продължи развитието на ``StudentManagement`` проекта.

Основният фокус беше върху:

синхронизацията между Model validation и database constraints;

премахването на зависимостта от ``Age``;

validation на ``FirstName``, ``LastName`` и ``Email``;

UI validation;

предварително архитектурно планиране на ``Course`` функционалността.

Целта беше първо да се стабилизира текущият Student модел и базата, 
преди да започне реализацията на Courses.

## 1. DateOfBirth и IsDateOfBirthEstimated
**Discussion**

Беше променен моделът за възрастта на студента.

Вместо да се съхранява Age, се използва реална дата на раждане:
````text
DateOfBirth
IsDateOfBirthEstimated
````

При съществуващи записи датата на раждане е изчислена приблизително на база на старата стойност на ``Age``.

``IsDateOfBirthEstimated`` позволява да се различава реална дата от приблизително изчислена дата.

**Decision**

Възрастта няма да бъде част от persistent модела.

Възрастта може да бъде изчислявана от DateOfBirth, когато е необходима.

**Implementation**

Добавена е migration:
````text
AddDateOfBirthAndEstimatedFlag
````

Migration-ът:

-добавя ``DateOfBirth``;

-добавя ``IsDateOfBirthEstimated``;

-преобразува съществуващите данни;

-маркира преобразуваните дати като estimated.

Migration-ът е приложен успешно към базата.

## 2. Премахване на Age
**Decision**

Полето:
````text
Age
````

не е необходимо да се съхранява в базата.

**Implementation**

``Age`` е премахнат от ``Student``.

Направена е migration:
````text
RemoveAgeFromStudent
````

Migration-ът е приложен успешно.

Колоната ``Age`` е премахната от таблицата ``Students``.

Методът ``CalculateAge()`` в ``StudentService`` остава, защото се използва за изчисляване на текущата възраст от ``DateOfBirth``.

## 3. FirstName validation
**Decision**

Ограниченията за ``FirstName`` трябва да бъдат централизирани в ``StudentConstants``.

Използват се:
````text
MinFirstNameLength = 2;
MaxFirstNameLength = 100;
````

Model validation:
````text
[Required]
[StringLength(
    StudentConstants.MaxFirstNameLength,
    MinimumLength = StudentConstants.MinFirstNameLength)]
````
**Database**

``FirstName`` вече използва:
````text
nvarchar(100)
````

вместо:
````text
nvarchar(max)
````

Това осигурява съответствие между application validation и database schema.

## 4. LastName validation
**Discussion**

Беше установено, че същото ограничение трябва да се приложи и към фамилията.

**Decision**

Добавени са:
````text
MinLastNameLength = 2;
MaxLastNameLength = 100;
````

``LastName`` използва:
````text
[Required]
[StringLength(
    StudentConstants.MaxLastNameLength,
    MinimumLength = StudentConstants.MinLastNameLength)]
````
**Database**

``LastName`` вече използва:
````text
nvarchar(100)
````

вместо:
````text
nvarchar(max)
````

Направена е migration:
````text
AddNameLengthConstraints
````

Migration-ът е приложен успешно.

## 5. Email validation
**Discussion**

Беше установено, че ``Email`` има application validation чрез:
````text
[Required]
[EmailAddress]
````

но database колоната все още беше:
````text
nvarchar(max)
````
**Decision**

Email трябва да има максимална дължина:
````text
254
````
**Implementation**

Email колоната е променена на:
````text
nvarchar(254)
````


Направена е migration:
````text
AddEmailLengthConstraint
````

Migration-ът е приложен успешно.

## 6. UI validation
**Implementation**

В Student Create/Edit UI са добавени визуални обозначения ``*``за задължителните полета.

UI validation е съобразена с Model validation.

``StudentConstants`` се използва и в UI, когато е необходимо.

## 7. Архитектурно обсъждане – Courses
**Idea**

Да бъде добавена отделна таблица за курсовете, вместо ``Student`` да съдържа свободен текст:
````text
Course
````
**Discussion**

Беше обсъдено, че един студент може да посещава повече от един курс.

Също така един курс може да има много студенти.

Следователно връзката не трябва да бъде:
````text
Student → Course
````

а:
````text
Student * ↔ * Course
````
**Decision**

Ще бъде използвана ``Many-to-Many`` връзка.

Концептуалният модел е:
````text
Students
    *
    |
    |
StudentCourses
    |
    |
    *
Courses
````
## 8. Course model – Version 1
**Discussion**

Бяха обсъдени различни варианти за идентификация на курса.

Първоначално беше разгледан вариант с кодове от типа:
````text
C#-101
2026-09-001
````

По време на обсъждането беше установено, че кодът не трябва да съдържа информация за:

-година;

-месец;

-преподавател;

-версия на курса.

Тази информация може да стане отделна бизнес концепция в бъдещи версии.

**Decision**

За Version 1 Course ще бъде концептуално:
````text
Courses
----------------
Id
Code
Name
Description
````

Code ще бъде стабилен код на курса.

Пример:
````text
CSHARP
SQL
ASPNET
````

Кодът няма да се използва за кодиране на година, месец или версия.

## 9. Course – бъдещо развитие
**Discussion**

Беше обсъден сценарий, при който един и същ курс се провежда многократно:
````text
C# Programming
    September 2026 – Teacher A
    January 2027   – Teacher B
    May 2027       – Teacher C
````

Това означава, че в бъдеще трябва да се разграничат:
````text
Course
````

и
````text
CourseOffering

Future Proposal
````
В бъдеща версия може да се появи:
````text
CourseOffering
----------------
Id
CourseId
Year
Month
TeacherId
StartDate
EndDate
````

Това няма да бъде реализирано в текущата версия.

## 10. Course versions and topics
**Discussion**

Беше обсъдено и развитието на съдържанието на даден курс.

Например:
````text
C# Programming
    Version 1
    Version 2
````

Различните версии могат да съдържат различни теми.
````text
Future Proposal
````
В бъдеща версия могат да бъдат въведени:
````text
CourseVersion
Topic
CourseTopics
````

Това няма да бъде включено в текущата реализация.

## 11. Course management
**Future Requirement**

Беше обсъдено, че Manager трябва да може да управлява курсовете.

Потенциални операции:

добавяне на курс;

редактиране на курс;

изтриване на курс;

преглед на курсове.

Това ще бъде реализирано след уточняване на окончателния модел.

## 12. Student – Course UI
**Decision**

При ``Sudent`` Create/Edit няма да се използва свободен текст за курс.

Тъй като връзката е ``Many-to-Many``, курсовете ще се избират чрез ``multi-select``.

Концептуално:
````text
Courses

☑ C# Programming
☐ SQL Server
☑ ASP.NET Core
☐ JavaScript
````

Един студент ще може да има множество избрани курсове.

## 13. Architecture
**Decision**

Съществуващата Layered Architecture се запазва.

Основният поток остава:
````text
View
  ↓
Controller
  ↓
Service
  ↓
ApplicationDbContext
  ↓
SQL Server
````

За ``Courses`` се предвиждат:
````text
CoursesController
CourseService
ICourseService
Course
````

и съответните Views.

**Repository Pattern**

Не се добавя Repository Pattern на този етап.

Съществуващият:
````text
Service → ApplicationDbContext
````

се счита за достатъчен за текущия проект.

**DTOs**

DTO слой няма да бъде добавян предварително.

Ще бъде въведен само ако бъде необходим при бъдещо усложняване на application flow-а.

## 14. Current State

Към края на работата:

-``DateOfBirth`` е реализиран;

-``IsDateOfBirthEstimated`` е реализиран;

-``Age`` е премахнат от базата;

-``CalculateAge()`` се използва за изчисляване на възрастта;

-``FirstName`` има validation 2–100;

-``LastName`` има validation 2–100;

-``FirstName`` е nvarchar(100);

-``LastName`` е nvarchar(100);

-``Email`` е nvarchar(254);

-Email има ``[Required]`` и ``[EmailAddress]``;

-UI показва ``*`` за задължителните полета;

-всички съответни migrations са приложени успешно;

-проектът Build-ва успешно.

## 15. Next Steps

Следващата функционалност е ``Courses``.

Преди реализацията трябва да се изгради:
````text
Course
   ↓
Many-to-Many
   ↓
Student
````

с концептуална структура:
````text
Student
   *
   ↕
StudentCourses
   ↕
   *
Course
````

Първата версия на ``Course`` ще съдържа:
````text
Id
Code
Name
Description
````

``Student`` Create/Edit ще използва multi-select за избиране на курсове.

## Функционалности като:

-CourseOffering;

-година;

-месец;

-преподаватели;

-CourseVersion;

-Topics;

-Enrollment details;

-остават за бъдещи версии и няма да бъдат включвани преждевременно.
