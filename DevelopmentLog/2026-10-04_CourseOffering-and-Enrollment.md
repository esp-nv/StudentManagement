## Development Log – 2026-10-04
## CourseOffering

Продължихме реализацията на CourseOffering като отделна domain функционалност между Course и Enrollment.

Създадени/използвани са:
````text
ICourseOfferingService
CourseOfferingService
CourseOfferingViewModel
CourseSelectItemViewModel
CourseOfferingsController
CourseOffering Views
````

CourseOffering поддържа:
````text
Index
Details
Create
Edit
Delete
````

Service layer-ът използва AsNoTracking() при четене и включва свързания Course чрез:
````text
Include(co => co.Course)
````

При стандартно извличане се филтрират soft-deleted CourseOfferings и Courses.

## CourseOffering Create / Edit

Create и Edit използват CourseOfferingViewModel, вместо директно да приемат domain model от UI.

ViewModel-ът съдържа:
````text
CourseId
StartDate
EndDate
EnrollmentStartDate
EnrollmentEndDate
Capacity
Courses
````

За Course dropdown е използван отделен:
````text
CourseSelectItemViewModel
````

който съдържа:
````text
Id
Code
Name
````

Така UI получава само необходимата информация за избора на Course.

## CourseOffering Validation

При тестване на Create формата беше открит проблем с ``[Required] ``върху DateTime.

Първоначално полетата бяха:
````text
DateTime StartDate
DateTime EndDate
DateTime EnrollmentStartDate
DateTime EnrollmentEndDate
````

Проблемът е, че DateTime е value type и при липса на стойност получава default:
````text
01.01.0001
````

Поради това ``[Required]`` не може да третира полето като null.

Решението беше полетата във ViewModel-а да станат nullable:
````text
DateTime?
````

като ``[Required] ``остана.

Validation логиката беше променена така, че първо да проверява дали стойностите съществуват.

Например:
````text
StartDate.HasValue
EndDate.HasValue
````

Това предотвратява validation logic да се опитва да сравнява липсващи стойности.

## CourseOffering Business Validation

В CourseOfferingViewModel е реализиран IValidatableObject.

Проверяват се:
````text
EndDate >= StartDate

EnrollmentEndDate >= EnrollmentStartDate

EnrollmentStartDate <= StartDate
````

При нарушение се добавя конкретна ValidationResult към съответното поле.

Това беше тествано през UI.

Например при:
````text
Start date: 05-Nov-2026
End date: 09-Feb-2026
````

се показва:
````text
End date cannot be before start date.
````

При:
````text
Enrollment start date: 15-Nov-2026
Start date: 05-Nov-2026
````

се показва:
````text
Enrollment start date cannot be after course start date.
````

При:
````text
Enrollment start date: 15-Nov-2026
Enrollment end date: 10-Nov-2026
````

се показва:
````text
Enrollment end date cannot be before enrollment start date.
````

Невалидните данни не се записват.

## Empty CourseOffering Form

При отваряне на празен Create form validation-ът вече работи коректно.

Показват се:
````text
The CourseId field is required.
The StartDate field is required.
The EndDate field is required.
The EnrollmentStartDate field is required.
The EnrollmentEndDate field is required.
Please enter a value greater than or equal to 1.
````

Това потвърди, че ``DateTime? + [Required] ``решава проблема с празните дати.

## CourseOffering Soft Delete

CourseOffering използва същия soft-delete подход като Course.

При Delete:
````text
IsDeleted = true
````

Записът остава в базата.

При стандартно извличане той вече не се показва.

Това беше проверено както през UI, така и директно в базата.

## CourseOffering Testing

CourseOffering CRUD беше проверен през реалния UI.

Проверени са:
````text
Create
Edit
Details
Delete
Invalid dates
Empty dates
Empty Course selection
Invalid Capacity
Soft delete
````

Проверено е и поведението на базата след операциите.

Build-ът на проекта завърши успешно без compilation errors.

## Enrollment Domain

След завършването на основната CourseOffering функционалност преминахме към Enrollment.

Потвърдена е архитектурната връзка:
````text
Student
   ↓
Enrollment
   ↓
CourseOffering
   ↓
Course
````

Student не се записва директно в Course.

Student се записва в конкретен CourseOffering.

Това позволява един Course да има множество CourseOfferings в различни периоди.

## Enrollment Model

Използва се Enrollment model със:
````text
Id
StudentId
CourseOfferingId
EnrollmentDate
Status
IsDeleted
Student
CourseOffering
````

Enrollment има собствен EnrollmentStatus enum:
````text
Pending
Active
Cancelled
Completed
Failed
NotAttended
````

Така статусът на записването е част от domain модела, а не произволен текст от UI.

## Enrollment Service

Създадени са:
````text
IEnrollmentService
EnrollmentService
EnrollmentsController
````

Service-ът към момента поддържа:
````text
GetByIdAsync
CreatePendingAsync
ApproveAsync
````
## Create Pending Enrollment

CreatePendingAsync извършва няколко проверки.

Проверява се:
````text
Student съществува
Student не е soft-deleted

CourseOffering съществува
CourseOffering не е soft-deleted

Student вече няма Enrollment за този CourseOffering
````

При успешно създаване се създава:
````text
Status = Pending
IsDeleted = false
````

EnrollmentDate се задава от application logic чрез:
````text
DateTime.UtcNow
````

Той не идва от UI.

## Duplicate Enrollment

Не се допуска един Student да има повече от един активен/съществуващ Enrollment за същия CourseOffering.

При вече съществуващ запис CreatePendingAsync връща неуспешен резултат.

Това е business rule и затова проверката е в Service layer, а не само в UI.

## Enrollment Approval

Реализирана е базова ApproveAsync логика.

Enrollment може да бъде одобрен само ако текущият статус е:
````text
Pending
````

Преди промяна към Active се проверява CourseOffering.

След това се преброяват съществуващите Active Enrollments.

Ако:
````text
Active Enrollments >= Capacity
````

операцията се отказва.

При свободно място:
````text
Status = Active
````

и Enrollment се записва.

## Enrollment ViewModel

На този етап не е добавен излишен Enrollment ViewModel.

Причината е, че първоначалният Enrollment flow все още се уточнява от гледна точка на UI.

Потвърдено е, че Student не трябва да може да задава директно:
````text
EnrollmentDate
Status
````

при първоначално записване.

Тези стойности са част от application/business logic.

ViewModel ще бъде добавен, когато започнем конкретния Student → Available Courses → Enroll UI flow.

## Student Details – бъдещ Enrollment UI

За тестови цели избрахме съществуващия:
````text
Students/Details
````

като начална точка за Enrollment функционалността.

Идеята е под информацията за Student да има секция:
````text
Available Courses
````

В горната част ще се показва:
````
First Name + Last Name
Email
````

След това ще има таблица с достъпните CourseOfferings.

## Available Courses

Първата версия няма да бъде система за търсене и филтриране на всички курсове.

Student трябва да вижда само CourseOfferings, които в момента са достъпни за записване.

Основната идея е да се показват CourseOfferings, при които:
````text
CourseOffering не е deleted
Course не е deleted
Enrollment period е започнал
Enrollment period не е приключил
Student няма съществуващ Enrollment
````

По този начин Student няма да вижда вече приключили възможности за записване в основния Available Courses UI.

Search и filtering на исторически/минали CourseOfferings остават за бъдещ етап.

## Student Enrollment Actions

Първоначалният Student UI ще бъде умишлено прост.

Основното действие ще бъде:
````text
Enroll
````

При записване:
````text
Pending
````

За вече записан CourseOffering ще бъде предвидено:
````text
Cancel
````

Това е началният функционален flow.

По-късно могат да бъдат добавени допълнителни действия според реалните business requirements.

## Student Details UI Design

Потвърдено е, че Students/Details ще бъде използван първоначално за функционално тестване на Enrollment.

Финалният дизайн на страницата няма да се счита за приключен на този етап.

След като функционалността бъде проверена, UI ще бъде преработен така, че да следва окончателния дизайн на приложението.

Това следва установения принцип:
````text
Първо функционалност
        ↓
Тестване
        ↓
Business validation
        ↓
Финален UI дизайн
````
## Validation и Separation of Concerns

Продължихме да следваме разделението на отговорностите:
````text
View / HTML
    ↓
Client-side UX

ViewModel / DataAnnotations
    ↓
Server-side validation

Controller
    ↓
HTTP flow

Service
    ↓
Business rules

EF Core / DbContext
    ↓
Persistence

Database
    ↓
Data integrity
````

Особено при Enrollment business rules не трябва да разчитаме само на UI.

Например Capacity проверката трябва да остане в Service layer, дори UI да показва свободните места.

## UI Consistency

Продължихме да следваме вече установения UI стандарт от Student и Course.

Новите форми трябва да използват съществуващия подход за:
````text
Headings
Bootstrap spacing
Form controls
Validation messages
Save / Cancel
Details / Edit / Delete
````

Ако в бъдеще бъде избран нов общ дизайн, съществуващите страници ще бъдат адаптирани към него.

На този етап не променяме установения UI само заради новата функционалност.

## Authorization

Authorization с Roles / Permissions остава бъдеща архитектурна посока.

Не е започната реална имплементация на Roles / Permissions.

Enrollment flow-ът е структуриран така, че по-късно да може да бъде поставен зад необходимите authorization rules.

## Текущо състояние

Към края на деня:
````text
Student CRUD UI
    ↓
функционален

Course CRUD UI
    ↓
функционален

Course soft delete
    ↓
проверен

CourseOffering CRUD
    ↓
функционален

CourseOffering validation
    ↓
проверена

CourseOffering soft delete
    ↓
проверен

Enrollment model
    ↓
готов

EnrollmentStatus
    ↓
готов

EnrollmentService
    ↓
Pending + Approve логика

EnrollmentsController
    ↓
базова структура
````

Следващата конкретна стъпка е:
````text
Students/Details
      ↓
Available Courses
      ↓
Enroll
      ↓
Pending
      ↓
Cancel
````

След това ще бъде направено функционално тестване през UI и директна проверка на данните в базата.

Финалният дизайн на Students/Details ще бъде преработен след като Enrollment flow-ът работи коректно.

Search / filtering на CourseOfferings и по-сложните Enrollment сценарии остават за следващ етап.
