## Development Log – 2026-10-03
## Student UI

Продължихме изграждането на Student UI върху вече съществуващия Student CRUD.

Добавихме и проверихме:
````text
Index

Details

Create

Edit

Delete
````
Student Index не използва paging на този етап.

В Index UI е добавен единен стандарт за действията:
````text
Details

Edit

Delete
````
Добавено е временно визуализиране на Student Id за development/debugging цели, което може да бъде премахнато по-късно.

## Student Create / Edit UI consistency

Уеднаквихме UI поведението между Create и Edit.

За текстовите полета използваме централизирани константи и character counter:
````text
First Name

Last Name

Course Code

Course Name

Description
````
Character counter-ът се показва при активиране на полето, а не постоянно.

Целта е еднакво поведение и визуален стандарт между Create и Edit формите.

## Student Date of Birth

Запазихме съществуващата специфична логика при Edit.

Когато датата на раждане е маркирана като estimated:
````text
полето остава readonly;

показва се предупреждение;

потребителят може да избере Update date of birth;

след активиране полето става editable;

задава се dateOfBirthConfirmed = true.
````
При Edit продължаваме да използваме server-side validation и съществуващата business логика.

## Course CRUD

Започнахме реализацията на Course CRUD.

Създадени/използвани са:
````text
CourseConstants

Course model

ICourseService

CourseService

CoursesController

Course Views
````
Course използва soft delete чрез IsDeleted.

При нормално четене на данните soft-deleted courses не трябва да се показват в потребителския UI.

Course UI следва същия общ стандарт като Student UI.

Използваме:
````text
еднакви Bootstrap класове;

еднакъв стил на бутоните;

Details, Edit и Delete;

централизирани validation constants;

character counters за подходящите текстови полета.
````
## Soft Delete

Потвърдихме, че проектът използва soft delete като основен подход там, където е необходимо да запазим исторически данни.

Това означава:
````text
IsDeleted = false
    ↓
нормален запис

IsDeleted = true
    ↓
записът остава в базата,
но не се показва в стандартния UI
````

Причината е да не губим историческа информация, която може да бъде необходима по-късно за анализи, справки или audit функционалност.

## Course – CourseOffering – Enrollment

Потвърдихме архитектурната връзка:
````text
Student
   ↓
Enrollment
   ↓
CourseOffering
   ↓
Course
````

Това означава, че студентът не е директно записан в Course, а в конкретно CourseOffering.

Course описва самия курс.

CourseOffering описва конкретно издание/провеждане на курса.

Enrollment описва записването на конкретен Student в конкретно CourseOffering.

При бъдещо изграждане на UI трябва да се вземе предвид IsDeleted както за Course, така и за свързаните записи, така че историческите данни да могат да бъдат запазени без автоматично да се показват като активни данни.

## Validation и Separation of Concerns

Продължихме подхода важните правила да не разчитат само на една проверка.

Различните нива имат различни отговорности:
````text
View / HTML
    ↓
client-side UX и basic ограничения

Model / DataAnnotations
    ↓
server-side validation

Controller
    ↓
HTTP request / response flow

Service
    ↓
application / business logic

EF Core / DbContext
    ↓
persistence

Database
    ↓
последна защита на data integrity
````

Целта е всеки слой да има конкретна отговорност.

Не се стремим просто да дублираме една и съща проверка навсякъде, а да поставим правилото на подходящото ниво според неговата отговорност.

## UI consistency

Установихме като принцип за проекта:

Когато вече имаме утвърден UI стандарт за една функционалност, новите подобни функционалности трябва да го следват, вместо всяка страница да има собствен стил.

Това включва:
````text
заглавия;

Bootstrap spacing;

form controls;

validation messages;

action buttons;

Details/Edit/Delete navigation;

character counters;

Cancel/Save поведение.
````
Ако дадена функционалност има специфична нужда, тя може да се различава, но разликата трябва да е обоснована от самата функционалност.

## Архитектурно мислене

Обсъдихме, че при разработването на проекта не е необходимо предварително да използваме терминологията на всички design patterns и architectural principles.

Първо идентифицираме проблема и желаното поведение, след което определяме дали вече съществува познат pattern или principle, който описва решението.

Пример:
````text
"Всеки файл трябва да има конкретна задача."
        ↓
Separation of Concerns
````

Друг пример:
````text
"Не искаме всяка нова роля да изисква промени
в множество файлове."
        ↓
необходимост от extensible authorization model
        ↓
възможен Role / Permission модел
````

Терминологията се използва като средство за комуникация и по-добро разбиране, а не като нещо, което трябва да бъде запомнено механично.

## Roles и Permissions – архитектурна посока

Обсъдихме бъдеща authorization архитектура, при която правата не са hard-coded в множество Controller-и.

Идеята е да могат да съществуват:
````text
Role
    ↓
Permissions

User
    ↓
individual permissions / overrides
````

Правата могат да бъдат свързани с конкретни области и действия, например:
````text
Students
Courses
Enrollments
````

и:
````text
Create
Read
Update
Delete
````

Това е само архитектурна посока на този етап.

Не започваме имплементация на Roles/Permissions преди да бъдат уточнени реалните business requirements и authorization правилата.

## Подход към проверките

Потвърдихме принципа, че промяната на данни трябва да преминава през подходящите нива на проверка, преди да достигне базата.

UI validation сама по себе си не се счита за security.

Backend трябва независимо да проверява:
````text
authentication;

authorization;

validation;

business rules;

съществуване на ресурса;

текущото състояние на ресурса;

ограниченията на domain модела.
````
След успешното преминаване на необходимите проверки операцията може да бъде изпълнена чрез service/persistence слоя.

## Текущо състояние

Student CRUD UI е функционален.

Student има:
````text
Index

Details

Create

Edit

Delete
````
Course CRUD е започнат и UI следва установения Student UI стандарт.

Използва се soft delete за Course.

Character counter поведението е уеднаквено между съответните Create/Edit форми.

Проектът продължава да следва разделяне на отговорностите между:
````text
Models
ViewModels
Controllers
Services
Views
JavaScript
Data / DbContext
Database
````

Authorization с Roles/Permissions е обсъдена като бъдеща архитектурна посока, но не е имплементирана на този етап.

Следващата естествена стъпка е да свържем съществуващите Course, CourseOffering, Student и Enrollment данни и да започнем реалната business logic за записването, вместо да добавяме UI без необходимата domain логика.
