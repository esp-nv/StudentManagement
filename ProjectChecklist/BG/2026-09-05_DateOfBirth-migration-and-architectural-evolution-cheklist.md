# Checklist — 2026-09-05

## DateOfBirth / Age

- [x] Обсъдено е защо `Age` не трябва да бъде основен database field.
- [x] Приета е посоката `DateOfBirth → Calculate Age`.
- [x] Добавено е `DateOfBirth` в `Student`.
- [x] `Age` е премахнато от `Student`.
- [x] Промяната е записана в commit `56cd178` (`add DateOfBirth`).
- [ ] `Age` е премахнато от всички останали application layers.
- [ ] `Age` е премахнато от `CreateStudentViewModel`.
- [ ] `EditStudentViewModel` е проверен.
- [ ] Validation е прехвърлена към `DateOfBirth`.
- [ ] Views са проверени.
- [ ] Controllers са проверени.
- [ ] Services са проверени.

## Database Migration

- [x] Обсъдена е постепенна migration strategy.
- [x] Определена е последователността:
  1. Add `DateOfBirth`
  2. Populate `DateOfBirth`
  3. Verify data
  4. Remove `Age`
  5. Calculate `Age` when needed
- [x] Установено е, че текущите database записи са тестови данни.
- [ ] Направена е migration за `DateOfBirth`.
- [ ] Съществуващите данни са прехвърлени към `DateOfBirth`.
- [ ] Данните са проверени след migration.
- [ ] `Age` е премахнато от database schema.
- [ ] EF Core migrations са приведени в съответствие с новия model.

## Architecture

- [x] Прегледана е посоката `View → ViewModel → Controller → Service → DbContext → Database`.
- [x] Установено е, че промяната на model-а трябва да бъде проследена през всички слоеве.
- [ ] Направен е пълен преглед на текущата implementation преди следващите migrations/refactoring.

## Student / Course

- [x] Потвърдена е посоката за `Student ↔ StudentCourse ↔ Course`.
- [x] `Course` е определен като отделна entity.
- [x] `StudentCourse` е определен като join entity.
- [ ] Определени са окончателните полета на `Course`.
- [ ] Определени са окончателните полета на `StudentCourse`.
- [ ] Определени са всички правила за enrollment.
- [ ] Направена е implementation на relationship-а.

## Version Planning

- [x] Започнато е оформянето на Version Roadmap.
- [x] V1 / V2 / V3 са разгледани като етапи на развитие.
- [x] Уточнено е, че Version Roadmap не е окончателен.
- [ ] V1 scope е окончателно уточнен.
- [ ] Version Roadmap е потвърден спрямо реалния code и database design.

## Project History

- [x] Приет е принципът да се запазва развитието на решенията.
- [x] Историята трябва да следва `Idea → Discussion → Decision → Implementation → Change → New Decision`.
- [ ] Historical Note за 05-09 е създаден по приетата бланка.
- [ ] Checklist за 05-09 е съгласуван с реалното Git състояние.

## Next Session

- [ ] Пълен преглед на текущия `StudentManagement` codebase.
- [ ] Проверка на всички останали `Age` references.
- [ ] Проверка на текущите EF Core migrations и database schema.
- [ ] Финализиране на database design преди следващи migrations/refactoring.
- [ ] След финализиране на design-а — планиране на реалната `DateOfBirth` migration.
