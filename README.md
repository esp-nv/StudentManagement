# 🎓 StudentManagement System (ASP.NET Core MVC)

Уеб приложение за управление на учебен процес, курсисти, курсове и оценки, разработено с **ASP.NET Core MVC**, **Entity Framework Core** и **SQL Server**.

> **🚧 Статус на проекта:** В активна разработка *(Work in Progress — 100+ комита)* — проектът се развива поетапно с детайлен дневник на разработката (`DevelopmentLog/`) и контролен списък (`ProjectChecklist/`).

---

## 🏗️ Архитектура и Документация в Репозиторито

* **`StudentManagement/`** — Основното ASP.NET Core MVC приложение (Controllers, Services, Data, Models, ViewModels, Views).
* **`DevelopmentLog/`** — Инженерен дневник на архитектурните решения и напредъка по етапи.
* **`ProjectChecklist/`** — Структуриран чеклист с функционални и технически изисквания.
* **`HistoricalNotes/ BG/`** — Бележки и анализи по време на проектирането на системата.

---

## 🛠️ Използвани Технологии

* **Backend:** C#, ASP.NET Core MVC, Dependency Injection
* **Database & ORM:** Microsoft SQL Server, Entity Framework Core (Migrations, Fluent API, LINQ)
* **Frontend:** Razor Pages / Views, HTML5, CSS3, Bootstrap
* **License:** MIT License

---

## 🗺️ План за Развитие (Roadmap)

- [x] Проектиране на релационна база данни (Students, Courses, Enrollments, Grades)
- [x] CRUD операции и валидация чрез ViewModels
- [x] Документиране на процеса в `DevelopmentLog` и `ProjectChecklist`
- [ ] Филтриране, търсене и пагинация на записите
- [ ] Роли и права за достъп (Admin, Teacher, Student) с ASP.NET Core Identity
- [ ] Автоматизирани тестове на бизнес логиката (xUnit)

---

## 🚀 Локално Стартиране

1. Клонирайте репозиторито:
   ```bash
   git clone https://github.com
   ```
2. Влезте в директорията на решението и приложете миграциите:
   ```bash
   dotnet ef database update
   ```
3. Стартирайте проекта:
   ```bash
   dotnet run
   ```
