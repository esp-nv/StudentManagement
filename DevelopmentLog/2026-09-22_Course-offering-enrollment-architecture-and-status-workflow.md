
## Development Log – 2026-09-22
## Поведение при изтриване на свързани записи

Прегледахме foreign key връзките и решихме да не използваме каскадно изтриване.

В ApplicationDbContext всички foreign key връзки са конфигурирани с DeleteBehavior.Restrict.

Така свързаните записи са защитени от нежелано каскадно изтриване.

Използването на IsDeleted остава предпочитаният подход за soft delete.

## Module

Добавихме entity Module.

Създадохме връзка:

StudyProgram → Module (one-to-many).

Добавихме IsDeleted.

Добавихме централизирани константи за ограниченията на името и описанието.

Добавихме ограничения:

Name – максимум 100 символа.

Description – максимум 500 символа.

Създадохме и приложихме migration AddModule.

Създадохме и приложихме migration UpdateModuleLengths.

Добавихме Module в ApplicationDbContext.

## Course

Добавихме entity Course.

Добавихме централизирани константи за валидация.

Добавихме ограничения:

Code – максимум 20 символа.

Name – максимум 100 символа.

Description – максимум 500 символа.

Добавихме IsDeleted.

Създадохме и приложихме migration AddCourse.

Решихме Course да остане максимално прост за V1.

## Връзка Module – Course

Създадохме many-to-many връзка между Module и Course.

Добавихме междинната таблица CourseModule.

И двете foreign key връзки са конфигурирани с Restrict.

Създадохме и приложихме migration AddModuleCourseRelation.

## CourseOffering

Добавихме CourseOffering като отделен entity, който представлява конкретно провеждане/издание на даден Course.

Добавихме:

CourseId

StartDate

EndDate

Capacity

IsDeleted

Добавихме изчисляемо свойство Year:

Year се извлича от StartDate.

Year не се записва като отделна колона в базата.

Добавихме application-level валидация чрез IValidatableObject.

Добавихме правило, че EndDate не може да бъде преди StartDate.

Същото правило е добавено и на ниво база чрез SQL Server CHECK constraint:

CK_CourseOffering_EndDate_After_StartDate

Създадохме и приложихме migration AddCourseOffering.

## Enrollment

Добавихме entity Enrollment.

Създадохме връзки:

Student → Enrollment

CourseOffering → Enrollment

Добавихме:

StudentId

CourseOfferingId

EnrollmentDate

Status

IsDeleted

Добавихме Enrollment в ApplicationDbContext.

## Статус на записването

Добавихме enum EnrollmentStatus в Common/Enums.

Зададохме изрични числови стойности на статусите, за да не се променят съществуващите стойности в базата при бъдещо добавяне или промяна на enum-а.

Pending = 1
Active = 2
Cancelled = 3
Completed = 4
Failed = 5
NotAttended = 6


Значение на статусите:

Pending – студентът е подал заявка, но тя още не е одобрена.

Active – заявката е одобрена и студентът е реално записан.

Cancelled – записването е отменено.

Completed – записването/курсът е приключил успешно.

Failed – студентът не е издържал.

NotAttended – студентът не се е явил на изпита.

## Защита от двойно записване

Добавихме уникален индекс върху:

StudentId

CourseOfferingId

Това не позволява един и същ студент да бъде записан два пъти в едно и също CourseOffering.

Създадохме и приложихме migration AddEnrollment.

Проверихме, че SQL Server успешно е създал уникалния индекс.

Пример:
````text
StudentId | CourseOfferingId
----------|-----------------
1         | 5               ✅
1         | 6               ✅
2         | 5               ✅
1         | 5               ❌
````
Процес на записване

Определихме първоначалния workflow за V1:
````text
Pending
   │
   ├──→ Active
   │
   └──→ Cancelled

Active
   │
   ├──→ Completed
   ├──→ Failed
   ├──→ NotAttended
   └──→ Cancelled
````

Pending означава подадена заявка от студента.

Одобрението от Manager или друг оторизиран потребител променя статуса на Active.

Completed, Failed и NotAttended представляват приключили състояния.

## Капацитет на CourseOffering

Определихме първоначалното правило за капацитета:

Само Active записванията заемат място.

Pending не заема място.

Cancelled не заема място.

Completed не заема текущ капацитет.

Failed не заема текущ капацитет.

NotAttended не заема текущ капацитет.

Например:

Capacity = 30
Active = 28
Pending = 7


Има още 2 свободни места.

Логиката за капацитета ще бъде част от business logic, а не от самия enum.

## CourseOffering – архитектура за V1

Решихме CourseOffering да остане максимално прост за V1.

Course описва самия курс.

CourseOffering описва конкретното издание/провеждане на курса.

Enrollment описва заявката и последващото записване на конкретен студент в това издание.

Основната връзка е:
````text
Student
   ↓
Enrollment
   ↓
CourseOffering
   ↓
Course
````

Умишлено не добавихме все още:

Online / OnSite / Hybrid

график

присъствие

домашни

изпити

плащания

оценки

Тези функционалности могат да бъдат добавени във V2, когато имаме реална нужда от тях.

## Валидация и защита на ниво база

Продължихме с подхода важните правила да бъдат защитени на няколко нива:
````text
UI / DataAnnotations
        ↓
Application / Business Logic
        ↓
EF Core
        ↓
Database
````

Примери:

ограничения на дължината на текстовите полета;

задължителни полета;

валидация на датите;

CHECK constraint за EndDate >= StartDate;

foreign key връзки с Restrict;

уникален индекс за StudentId + CourseOfferingId.

## Текущо състояние

Проектът се компилира успешно.

Всички създадени днес migrations са приложени успешно чрез Update-Database.

V1 вече има основата за:

области;

учебни програми;

модули;

курсове;

издания на курсове;

заявки/записвания на студенти;

статуси на записванията;

защита от двойно записване;

основни правила за капацитет.

В момента UI има CRUD функционалност за Student.

Следващата стъпка е да реализираме бизнес логиката за процеса на записване, започвайки от Pending → Active.
