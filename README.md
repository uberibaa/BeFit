# BeFit — dziennik treningowy

Projekt zaliczeniowy ASP.NET Core MVC. Umożliwia prowadzenie dziennika treningowego, zarządzanie typami ćwiczeń i analizę własnych wyników z ostatnich czterech tygodni.

## Technologie

- .NET 10, ASP.NET Core MVC i Razor Pages (Identity)
- Entity Framework Core 10, SQL Server LocalDB
- ASP.NET Core Identity, role użytkowników
- Bootstrap, Razor, CSS

## Funkcjonalności i dostęp

| Funkcja | Gość | Zalogowany użytkownik | Administrator |
|---|---|---|---|
| Katalog typów ćwiczeń i szczegóły | Tak | Tak | Tak |
| Dodawanie, edycja i usuwanie typów | Nie | Nie | Tak |
| Własne sesje treningowe i ćwiczenia | Nie | Tak | Tak |
| Własne statystyki | Nie | Tak | Tak |

Administrator nie otrzymuje dostępu do cudzych sesji ani wykonanych ćwiczeń. Usunięcie sesji usuwa powiązane wykonania; usuwanie używanego typu ćwiczenia jest blokowane.

## Uruchomienie lokalne (Windows)

1. Zainstaluj Visual Studio 2026 z obciążeniem **ASP.NET i tworzenie aplikacji internetowych**, .NET 10 SDK oraz SQL Server Express LocalDB.
2. Otwórz `BeFit.slnx`. Przywróć pakiety NuGet.
3. W konsoli menedżera pakietów NuGet wybierz projekt BeFit i wykonaj `Update-Database`. Migracje są już w repozytorium — nie generuj ich ponownie.
4. Kliknij prawym przyciskiem projekt BeFit → **Zarządzaj wpisami tajnymi użytkownika** (Manage User Secrets). Wstaw poniższą strukturę, używając własnego lokalnego adresu testowego i silnego hasła (nie zatwierdzaj tego pliku w Git):

   ```json
   {
     "Admin": {
       "Email": "admin@example.test",
       "Password": "<własne silne hasło testowe>"
     }
   }
   ```

5. Uruchom profil `https` (F5). Adres domyślny: https://localhost:7237. Jeżeli certyfikat nie jest zaufany, wykonaj lokalnie `dotnet dev-certs https --trust`.
6. Administrator jest tworzony przy uruchomieniu w środowisku Development; konto zwykłego użytkownika można zarejestrować w aplikacji. Użyj danych testowych.

Alternatywnie, w terminalu w katalogu projektu:

```powershell
dotnet restore
dotnet build
dotnet run --launch-profile https
```

Jeżeli baza już istnieje, `Update-Database` zastosuje tylko brakujące migracje. Zachowano dotychczasową nazwę bazy i identyfikator User Secrets, więc dotychczasowe lokalne dane powinny pozostać dostępne na tym samym komputerze. Dane bazy oraz User Secrets nie są częścią repozytorium.

## Model danych

- `ExerciseType` — nazwa i opcjonalny opis typu ćwiczenia.
- `WorkoutSession` — czas rozpoczęcia i zakończenia, właściciel (`IdentityUser`).
- `WorkoutExercise` — typ ćwiczenia, sesja, ciężar w kg, liczba serii i powtórzeń.

Sesja ma wiele wykonań ćwiczeń, a typ może występować w wielu wykonaniach. Właściciela wykonania określa właściciel jego sesji. Modele formularzy ograniczają pola przyjmowane z żądań POST. Kontrolery weryfikują właściciela także przy bezpośrednim dostępie przez URL.

## Statystyki

Dane obejmują własne sesje rozpoczęte między bieżącą chwilą minus 28 dni a bieżącą chwilą. Dla każdego typu ćwiczenia:

- liczba wykonań = liczba zapisanych rekordów wykonania;
- łączna liczba powtórzeń = suma iloczynów serii i powtórzeń w serii;
- średnie obciążenie = średnia arytmetyczna ciężarów z rekordów;
- maksymalne obciążenie = największy zapisany ciężar.

Brak wykonań oznacza zero w licznikach i kreskę zamiast ciężaru. Wartość 0 kg jest prawidłowa dla ćwiczeń bez dodatkowego obciążenia.

## Ograniczenia wersji edukacyjnej

Aplikacja jest przeznaczona do lokalnej prezentacji, a nie publicznego wdrożenia. W środowisku Development nie jest wymagane potwierdzanie adresu e-mail. Nie skonfigurowano rzeczywistej wysyłki e-mail ani zewnętrznych dostawców logowania. Funkcje zależne od e-maila nie są gotowe do użycia. Nie umieszczaj haseł, danych osobowych ani bazy danych w repozytorium. Przed publikacją uruchom kompilację i testy zgodnie z listą w `docs/TESTY.md`.
