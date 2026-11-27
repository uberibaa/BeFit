# Testy akceptacyjne BeFit

Poniższe scenariusze wymagają działającej aplikacji na Windows z LocalDB. Użyj wyłącznie kont i danych testowych. Status: do wykonania po audycie statycznym.

## Uruchomienie

- [ ] `dotnet build` kończy się bez błędów.
- [ ] `Update-Database` kończy się bez błędów.
- [ ] Aplikacja startuje, menu i formularze działają, brak wyjątków Identity.
- [ ] Administrator loguje się i może wejść na stronę Moje konto. Zwykły użytkownik może się zarejestrować, zalogować i wylogować.

## Katalog i uprawnienia

- [ ] Gość widzi katalog i szczegóły typu, nie widzi przycisków modyfikacji.
- [ ] Zwykły użytkownik otrzymuje odmowę dostępu do Create/Edit/Delete typu (również POST z ważnym tokenem CSRF).
- [ ] Administrator dodaje, edytuje i usuwa nieużywany typ; zbyt krótka nazwa jest odrzucana.
- [ ] Próba usunięcia typu przypisanego do wykonania daje komunikat, a rekord pozostaje w bazie.

## Sesje i wykonania

- [ ] Użytkownik A tworzy sesję; odwrócone lub równe daty są odrzucane.
- [ ] A dodaje wykonania; listy rozwijane pokazują nazwy typów i daty własnych sesji, nie surowe ID.
- [ ] A tworzy i edytuje wykonanie 82.5 kg, 0 kg; wartości ujemne, ponad limit i brak wymaganych pól są odrzucane. Sprawdź zapis z klawiaturą ustawioną na język polski.
- [ ] Szczegóły sesji zawierają tylko przypisane wykonania; dodanie ćwiczenia wybiera sesję.
- [ ] Użytkownik B nie widzi sesji ani wykonań A; wejście przez bezpośredni URL Details/Edit/Delete kończy się 404.
- [ ] B, posiadając własny token CSRF, nie może wysłać POST Edit/Delete z identyfikatorem sesji lub wykonania A. Nie może przypisać własnego wykonania do sesji A ani utworzyć w niej nowego.
- [ ] Żądania POST bez poprawnego tokenu CSRF są odrzucane.
- [ ] Usunięcie własnego wykonania działa; usunięcie własnej sesji usuwa jej wykonania.

## Statystyki

W sesjach użytkownika A rozpoczętych w ostatnich 28 dniach zapisz dla jednego typu:

| Ciężar | Serie | Powtórzenia |
|---:|---:|---:|
| 80 | 4 | 8 |
| 82.5 | 3 | 8 |

- [ ] Oczekiwany wynik: 2 wykonania, 56 powtórzeń, średnio 81.25 kg, maksymalnie 82.5 kg.
- [ ] Dodanie wykonania użytkownika B, sesji starszej niż 28 dni i sesji przyszłej nie zmienia wyników A.
- [ ] Typ bez wykonań ma 0 wykonań i powtórzeń, brak średniej/maksimum.
- [ ] Gość nie może otworzyć statystyk; B widzi tylko swoje dane.

## Publikacja

- [ ] Git ignoruje `bin`, `obj`, `.vs`, pliki `.user`, bazę danych oraz `secrets.json`.
- [ ] W repozytorium znajdują się kod, migracje, statyczne zasoby, README i plik rozwiązania; brak haseł i prywatnych danych.
