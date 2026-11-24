# 🎓 StudentManagement System (ASP.NET Core MVC)

Уеб приложение за управление на учебен процес, образователни програми, курсове, конкретни провеждания и записвания на курсисти, изградено с **ASP.NET Core MVC**, **Entity Framework Core** и **Microsoft SQL Server**.

> **🚀 Еволюция на проекта:** Започна като базов CRUD прототип и се разви в структурирана многослойна система с реална академична йерархия, 14 Code-First миграции, двойна защита на данните (в модела и в SQL Server) и подробен инженерен дневник на всяко архитектурно решение.

---

## 🏗️ Домейн Архитектура и Релационен Модел

Системата разграничава общата учебна програма (`Course`) от конкретното ѝ провеждане по дати и капацитет (`CourseOffering`), както и жизнения цикъл на всяко записване (`Enrollment`):

```text
Area (Област)
 └── 1 : N ──► StudyProgram (Учебна програма)
                └── 1 : N ──► Module (Модул)
                               └── N : N ──► Course (Курс)
                                              └── 1 : N ──► CourseOffering (Провеждане)
                                                             └── 1 : N ──► Enrollment (Записване)
                                                                            ▲
                                                                            │ N : 1
                                                                          Student (Студент)
```

### 🛡️ Защита в дълбочина (Defense in Depth) и Бизнес Правила
* **Защита срещу двойно записване:** Композитен уникален индекс в SQL Server (`IX_Enrollments_StudentId_CourseOfferingId`) гарантира на ниво база данни, че един студент не може да бъде записан два пъти в едно и също провеждане.
* **Валидация на времеви интервали (`EndDate >= StartDate`):** Двойна проверка чрез `IValidatableObject` (`yield return`) на ниво приложение и `CHECK CONSTRAINT` в базата данни (`CK_CourseOffering_EndDate_After_StartDate`).
* **Управление на капацитета:** Само записвания със статус `Active` заемат места от `Capacity` (докато `Pending`, `Cancelled`, `Completed`, `Failed` и `NotAttended` не блокират свободни места).
* **Защита на целостта на данните (`Soft Delete` & `Restrict`):** Глобално конфигуриран `DeleteBehavior.Restrict` (`ON DELETE NO ACTION`) върху всички външни ключове в комбинация с флаг `IsDeleted`.

---

## 📂 Структура на Репозиторито

* **`StudentManagement/`** — Основното ASP.NET Core MVC приложение:
  * `Controllers/` — Обработка на HTTP заявките (`Students`, `Courses`, `CourseOfferings`, `Enrollments`).
  * `Services/` & `Services/Interfaces/` — Изолирана бизнес логика и договори (`IStudentService`, `ICourseService`, `ICourseOfferingService`, `IEnrollmentService`).
  * `Data/` & `Migrations/` — `ApplicationDbContext` и 14 последователни Code-First EF Core миграции.
  * `Models/` & `ViewModels/` — Домейн сущности и специализирани ViewModels.
  * `Common/` — Централизирани константи (`Constants/`), енумерации (`EnrollmentStatus`) и валидационни съобщения.
* **`DevelopmentLog/`** — Хронологичен дневник на разработката и техническите решения.
* **`HistoricalNotes/BG/`** — Детайлни архитектурни анализи и обосновки (на български език).
* **`ProjectChecklist/`** — Структурирани контролни списъци по етапи на разработка.

---

## 🛠️ Използвани Технологии

* **Backend:** C# 12, .NET 8, ASP.NET Core MVC, Dependency Injection
* **Database & ORM:** Microsoft SQL Server, Entity Framework Core (Code-First, Fluent API, LINQ)
* **Frontend:** Razor Views, HTML5, CSS3, Bootstrap 5, JavaScript
* **License:** MIT License

---

## 🗺️ План за Развитие (V1 ➔ V2 Roadmap)

### ✅ Version 1 (Текуща основа — Core Academic Engine)
- [x] Йерархичен модел от 7 свързани сущности и 14 EF Core миграции
- [x] Разделение на отговорностите чрез `Services/Interfaces` и `ViewModels`
- [x] Двустепенна валидация и ограничения на ниво SQL Server
- [x] Пълни CRUD операции за студенти, курсове и провеждания
- [ ] Филтриране, търсене и пагинация на записите
- [ ] Покритие на бизнес логиката с модулни тестове (**xUnit**)

### 🔮 Version 2 (Бъдещи разширения)
- [ ] Ролева сигурност (**RBAC** — Admin, Teacher, Student) с ASP.NET Core Identity
- [ ] **AuditLog** (история на промените по полета: стара/нова стойност, потребител, дата) и **SystemLog** (`CorrelationId`)
- [ ] Графици, присъствия, оценки и модулни оформления на интерфейса (Sidebar / Navbar / Custom Layout)

---

## 🚀 Локално Стартиране

1. Клонирайте репозиторито:
   ```bash
   git clone https://github.com
   ```
2. Влезте в директорията на проекта:
   ```bash
   cd StudentManagement/StudentManagement
   ```
3. Приложете миграциите към локалния SQL Server:
   ```bash
   dotnet ef database update
   ```
4. Стартирайте приложението:
   ```bash
   dotnet run
   ```
