# Development Log – 2026-09-20

## Архитектура на Student – Course

Първоначалната директна връзка между `Student` и `Course` беше премахната.

`Student` вече няма директна navigation property към `Course`. Съответните референции към `Course` бяха премахнати и от Student ViewModel, controller, service и свързаните views.

Причината за промяната е, че студентът не трябва да бъде директно свързан с дефиницията на курса.

`Course` описва самия курс, докато конкретното провеждане на курса принадлежи към определен период.

Затова връзката се изгражда чрез `Enrollment` и `CourseOffering`.

Крайната концепция е:

```text
Student
    |
    v
Enrollment
    |
    v
CourseOffering
    |
    v
Course
    |
    v
StudyProgram
    |
    v
Area
````
## **Area**
Въведен е нов entity ``Area``, който представлява най-високото ниво на академично групиране.

Структурата на ``Area`` е:
````text
Area
 ├── Id
 ├── Code
 ├── Name
 └── Description
````
За свойствата на ``Area`` са определени validation constants.

``Area`` е добавен към ``ApplicationDbContext``.

Предстои създаването и прилагането на migration, както и проверка на структурата в базата данни.

## **StudyProgram**
Следващото ниво в йерархията е ``StudyProgram``.

Всеки ``StudyProgram`` принадлежи към определен ``Area``.

Концепцията е:
````text
Area
    |
    +── StudyProgram
            |
            +── Course
````
StudyProgram ще съдържа:
````text
StudyProgram
 ├── Id
 ├── AreaId
 ├── Code
 ├── Name
 └── Description
````
Validation constants ще бъдат отделени в:
````text
StudyProgramConstants.cs
````
Те ще определят допустимите дължини за ``Code, Name и Description``.

``StudyProgram`` ще има navigation property към Area и колекция от Course.

## **Course**
``Course`` представлява общата дефиниция на курса.

Той не е свързан с конкретна година, месец или отстъпка.

Предвидената структура е:
````text
Course
 ├── Id
 ├── StudyProgramId
 ├── Code
 ├── Name
 └── Description
````
``Course.Code`` съдържа единствено кода на курса.

В ``Course.Code`` няма да се включват:

-година;

-месец;

-отстъпка.

Тези данни принадлежат на CourseOffering.

## **CourseOffering**
``CourseOffering`` представлява конкретно провеждане на даден курс.

Един ``Course`` може да има множество ``CourseOffering`` записи.

Например:
````text
Course
    |
    +── CourseOffering – 2026 / 01
    |
    +── CourseOffering – 2026 / 06
    |
    +── CourseOffering – 2027 / 01
````
Предвидената структура е:
````text
CourseOffering
 ├── Id
 ├── CourseId
 ├── Year
 ├── Month
 └── DiscountPercent
````
``DiscountPercent`` представлява отстъпката за конкретното провеждане на курса.

Допустимият диапазон е 0–100.

## **Enrollment**
``Enrollment`` представлява записването на конкретен студент в конкретно провеждане на курс.

Студентът не се записва директно в ``Course``.

Връзката е:
````text
Student
    |
    v
Enrollment
    |
    v
CourseOffering
    |
    v
Course
````
Предвидената структура е:
````text
Enrollment
 ├── Id
 ├── StudentId
 ├── CourseOfferingId
 └── EnrollmentDate
````
При необходимост по-късно може да бъде добавен и статус на записването.

## **Крайна домейн структура**
Текущата концепция на домейна е:
````text
Area
  |
  +-- StudyProgram
        |
        +-- Course
              |
              +-- CourseOffering
                    |
                    +-- Enrollment
                          |
                          +-- Student
````
Тази структура разделя ясно:

-академичната област;

-учебната програма;

-дефиницията на курса;

-конкретното провеждане на курса;

-записването на студента.

По този начин информацията за година, месец и отстъпка не се смесва с основната дефиниция на курса.

## Текущо състояние
Промените по Student и неговата validation логика са завършени.

Директната връзка ``Student → Course`` е премахната.

Area е създаден и добавен към EF Core контекста.

Следващата стъпка е реализацията на ``StudyProgram``.

Работата ще продължи последователно, като след всяка основна промяна ще се правят build и проверка на базата данни.

## Следваща стъпка
Първо се създава:
````text
StudyProgramConstants.cs
````
След това:
````text
StudyProgram
    ↓
EF Core configuration
    ↓
DbSet<StudyProgram>
    ↓
Migration
    ↓
Проверка на базата данни
    ↓
Build
````
