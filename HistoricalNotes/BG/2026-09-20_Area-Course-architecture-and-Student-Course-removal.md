## Historical Note
## 2026-09-20 – Area, Course architecture and Student-Course removal
## Context

Продължи развитието на StudentManagement проекта.

Основният фокус беше върху:

премахването на старото Student.Course;

изграждането на първата реална част от новата Course архитектура;

въвеждането на Area;

синхронизацията между Model validation и database constraints;

създаването и прилагането на първата migration за Area;

уточняването на V1 структурата Area → Program → Course → CourseOffering → Enrollment → Student.

Целта беше първо да се изчисти старият Student.Course модел и след това постепенно да се изгради новата структура, като системата остане проста и чиста за текущия етап на разработка.

## 1. Премахване на Student.Course
**Discussion**

Първоначалният Student модел все още съдържаше:

Course


като свободен текст.

Това вече не съответстваше на новата архитектура, при която курсът трябва да бъде отделен domain модел.

Беше установено, че старото Student.Course се използва не само в модела, но и в:

StudentsController.cs
StudentService.cs
Delete.cshtml
Index.cshtml

Decision

Student няма да съхранява директно курс.

Старото:

Student
   └── Course


се премахва.

Курсовете ще бъдат свързвани със студентите чрез отделен модел, съобразен с бъдещата V1 архитектура.

Implementation

Course беше премахнат от:
````text
Student.cs
StudentsController.cs
StudentService.cs
Views/Students/Delete.cshtml
Views/Students/Index.cshtml
````

След това беше направена допълнителна проверка за останали зависимости.

Бяха открити и премахнати останалите визуални елементи, свързани със старото поле Course.

Проектът след промените се компилира успешно.

## 2. Course architecture – V1
**Discussion**

Беше преразгледана архитектурата, обсъдена на 19.09.

Първоначалната идея беше:
````text
Student
   *
   ↕
StudentCourse
   ↕
   *
Course
````text

По време на по-нататъшното обсъждане стана ясно, че системата трябва да може да представя по-ясно различните нива:
````text
Area
   ↓
Program
   ↓
Course
   ↓
CourseOffering
   ↓
Enrollment
   ↑
Student
````
**Decision**

За текущата V1 се приема:
````text
Area
   ↓
Program
   ↓
Course
   ↓
CourseOffering
   ↓
Enrollment
   ↑
Student
````

Enrollment представлява конкретното записване на конкретен студент за конкретно провеждане на курс.

Това позволява по-късно да се съхраняват независимо статус, дата на записване и отстъпка за конкретното записване.

## 3. Area
**Discussion**

Преди създаването на Area беше проверено дали в проекта вече съществува такъв модел.

Не беше намерен Area, но съществуваше празен:
````text
Models/Course.cs
````

Файлът не съдържаше реална Course реализация.

**Decision**

Празният Course.cs беше използван като основа за новия Area модел.

Така вместо да се създава излишен втори празен файл, съществуващият файл беше преименуван на:
````text
Area.cs
````

Новият модел е:
````text
Area
----------------
Id
Code
Name
Description
````
**Implementation**

Course.cs беше преименуван на:
````text
Area.cs
````

и беше реализиран моделът:
````text
public class Area
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
````

Първоначално моделът беше създаден без database configuration, след което постепенно бяха добавени validation и EF Core configuration.

## 4. AreaConstants
**Decision**

Както при Student, ограниченията за Area се отделят в собствен клас с константи.

Създаден е:
````text
AreaConstants.cs
````

с:
````text
MinCodeLength = 2
MaxCodeLength = 20

MinNameLength = 2
MaxNameLength = 100

MaxDescriptionLength = 500
````

Всеки domain модел има собствени constants, когато има собствена отговорност.

Така бъдеща промяна в ограниченията на Area няма да засяга други модели.

## 5. Area validation
**Decision**

Validation-ът на Area използва AreaConstants.

За Code:
````text
Required
2–20 characters


За Name:

Required
2–100 characters


За Description:

maximum 500 characters
````
**Implementation**

В Area.cs беше добавен DataAnnotations validation чрез:
````text
[Required]
[StringLength(...)]
````

като конкретните стойности идват от:
````text
AreaConstants
````

Така application validation и бъдещите database constraints използват едни и същи ограничения.

## 6. ApplicationDbContext
**Decision**

Area трябва да бъде регистриран в Entity Framework Core чрез съществуващия ApplicationDbContext.

**Implementation**

Добавен е:
````text
public DbSet<Area> Areas { get; set; } = null!;
````

към:
````text
ApplicationDbContext
````

След промяната проектът Build-ва успешно.

## 7. Migration – AddArea
**Discussion**

След като Area беше готов като model, validation и DbContext entity, беше създадена първата migration за новата структура.

**Implementation**

Създадена е:
````text
AddArea
````

Migration-ът съдържа две основни промени:

премахва старото:
````text
Students.Course
````

и създава:
````text
Areas
````

с:
````text
Id
Code
Name
Description
````

Database типовете и ограниченията са:
````text
Code        nvarchar(20)
Name        nvarchar(100)
Description nvarchar(500)
````

Migration-ът беше проверен преди да бъде приложен към database.

## 8. Update-Database
**Implementation**

Migration-ът беше приложен чрез:
````text
Update-Database
````

EF Core отчете:
````text
Applying migration '20260920180430_AddArea'.
````

След изпълнението:
````text
Done.
````

В database:
````text
Students
   └── Course ❌
````

е премахнато.

Създадена е:
````text
Areas
   ├── Id
   ├── Code
   ├── Name
   └── Description
````
## 9. Проверка на Areas
**Implementation**

След Update-Database таблицата Areas беше проверена в database.

Потвърдено беше, че таблицата съществува с очакваните колони и ограничения.

Така целият поток:
````text
Area Model
   ↓
AreaConstants
   ↓
Validation
   ↓
ApplicationDbContext
   ↓
Migration
   ↓
Database
````

е реализиран успешно.

## 10. Program
**Discussion**

След завършването на Area беше направена проверка за съществуващ Program.

В проекта вече съществува:
````text
StudentManagement.Program
````

който е startup класът на ASP.NET Core приложението.

Беше обсъдено дали domain моделът да се нарича StudyProgram, за да няма объркване.

**Decision**

StudyProgram няма да бъде използван.

Domain моделът остава:
````text
Program
````

Причината е, че:
````text
StudentManagement.Program
````

и:
````text
StudentManagement.Models.Program
````

са различни класове в различни namespaces и имат различни отговорности.

Program в StudentManagement стартира приложението.

Program в StudentManagement.Models ще представлява учебна програма.

Така не се променя domain терминологията само заради техническо неудобство.

## 11. Разделяне на отговорностите
**Discussion**

Беше обсъден принципът всяко различно бизнес понятие да има собствена отговорност.

В текущия модел:
````text
Area
Program
Course
CourseOffering
Enrollment
Student
````

представляват различни понятия.

Същият принцип се прилага и към constants:
````text
AreaConstants
ProgramConstants
CourseConstants
CourseOfferingConstants
EnrollmentConstants
StudentConstants
````
**Decision**

Не се създава една обща Constants класа, която да съдържа ограниченията на всички модели.

Всеки модел ще има собствен набор от constants.

Това позволява ограниченията да се променят независимо, когато бизнес изискванията го наложат.

## 12. Текущо състояние на архитектурата
**Decision**

Към този момент V1 архитектурата е:
````text
Area
   ↓
Program
   ↓
Course
   ↓
CourseOffering
   ↓
Enrollment
   ↑
Student
````

Основното правило за Enrollment е:
````text
Enrollment =
един Student
+
едно конкретно CourseOffering
````

DiscountPercent принадлежи на Enrollment, а не на отделна Discount таблица.

Концептуално:
````text
Enrollment
----------------
Id
StudentId
CourseOfferingId
Status
EnrollmentDate
DiscountPercent
````

Това позволява различни записвания на един и същ студент да имат различен статус и различна отстъпка.

## 13. Course selection
**Decision**

В бъдещия Student UI курсовете няма да се въвеждат като свободен текст.

Изборът ще бъде чрез checkbox/multi-select интерфейс.

Концептуално:
````text
C# Development

☑ Fundamentals
☑ OOP
☐ Advanced
````

Избраните курсове ще водят до съответните Enrollment записи.

Това позволява един студент да има множество активни записвания.

## 14. Бъдеща модулна архитектура
**Future Proposal**

Беше обсъдена бъдеща възможност различните части на системата да могат да работят независимо.

Например в бъдеща версия:
````text
Student Module
Area Module
Program Module
Course Module
````

могат да бъдат разделени така, че проблем в един модул да не спира напълно останалите.

Това би изисквало допълнителна архитектура, например независимо deployment-ване и комуникация между модулите.

Това не се реализира в текущата версия.

Системата все още се разработва и текущият принцип е:
````text
Изискване
   ↓
Най-просто решение
   ↓
Ясна отговорност
   ↓
Работещ код
   ↓
При ново изискване → разширяване
````

Не се добавят предварително компоненти само заради възможни бъдещи сценарии.

## 15. Current State

Към края на работата:

-Student.Course е премахнат;

-старите зависимости от Student.Course са премахнати;

-Area.cs е реализиран;

-AreaConstants.cs е създаден;

-Area има validation;

-Area е добавен в ApplicationDbContext;

-migration AddArea е създадена;

-migration AddArea е приложена успешно;

-таблицата Areas е създадена;

-Areas е проверена;

-ограниченията на Code, Name и Description са проверени;

-старото Students.Course е премахнато от database;

-проектът Build-ва успешно;

-Program е потвърден като отделен domain модел от startup Program;

-StudyProgram не се добавя;

-принципът за отделни отговорности е потвърден.
 
## 16. Next Steps

Следващата функционалност е Program.

Първоначално ще бъде изграден:
````text
ProgramConstants
        ↓
Program
        ↓
ApplicationDbContext
        ↓
Migration
        ↓
Database
````

След това архитектурата ще продължи към:
````text
Area
   ↓
Program
   ↓
Course
   ↓
CourseOffering
   ↓
Enrollment
   ↑
Student
````

Функционалности като:
````text
CourseOffering
Teacher
CourseVersion
Topics
Enrollment details
````


ще бъдат реализирани само когато бъдат необходими за следващия етап на проекта.
