# BeFit

BeFit to projekt zaliczeniowy z programowania zaawansowanego. Jest to aplikacja webowa do zapisywania treningów i sprawdzania swoich postępów.

## Funkcje aplikacji

- Przeglądanie katalogu ćwiczeń.
- Dodawanie, edycja i usuwanie własnych treningów oraz wykonanych ćwiczeń.
- Zapisywanie ciężaru, liczby serii i powtórzeń.
- Statystyki z ostatnich 28 dni.
- Rejestracja i logowanie użytkowników.

Katalog ćwiczeń jest dostępny również bez logowania, ale tylko administrator może go edytować. Każdy zalogowany użytkownik ma dostęp wyłącznie do własnych treningów, ćwiczeń i statystyk.

## Technologie

C#, .NET 10, ASP.NET Core MVC, Entity Framework Core, SQL Server LocalDB, ASP.NET Core Identity, Razor i Bootstrap.

## Jak uruchomić projekt?

Potrzebne są Visual Studio 2026 z obsługą ASP.NET, .NET 10 SDK i SQL Server LocalDB.

1. Otwórz plik `BeFit.slnx` w Visual Studio i przywróć pakiety NuGet.
2. W konsoli menedżera pakietów NuGet wykonaj polecenie `Update-Database`.
3. Uruchom projekt (F5, profil HTTPS).

Domyślny adres aplikacji: https://localhost:7237.

Jeśli pojawi się problem z certyfikatem HTTPS, wykonaj polecenie `dotnet dev-certs https --trust`.

### Konto administratora

Aby utworzyć administratora, kliknij prawym przyciskiem myszy projekt BeFit i wybierz **Manage User Secrets** (Zarządzaj wpisami tajnymi użytkownika). Dodaj własne dane w poniższym formacie:

```json
{
  "Admin": {
    "Email": "admin@example.test",
    "Password": "<własne silne hasło>"
  }
}
```

Konto administratora zostanie utworzone przy uruchomieniu aplikacji w środowisku Development. Zwykłe konto można założyć przez formularz rejestracji. Dane logowania i lokalna baza danych nie znajdują się w repozytorium.

## Modele

- `ExerciseType` – nazwa i opis ćwiczenia.
- `WorkoutSession` – data i czas rozpoczęcia oraz zakończenia treningu, a także jego właściciel.
- `WorkoutExercise` – ćwiczenie przypisane do treningu, ciężar, liczba serii i powtórzeń.

Usunięcie treningu powoduje również usunięcie jego ćwiczeń. Nie można usunąć z katalogu typu ćwiczenia, który jest już używany w treningach.

Statystyki uwzględniają własne sesje rozpoczęte w ostatnich 28 dniach. Liczba wykonań to liczba zapisanych rekordów danego typu ćwiczenia. Łączna liczba powtórzeń jest sumą iloczynów serii i powtórzeń. Wyświetlane są również średni i maksymalny ciężar.

## Uwagi

Projekt jest przeznaczony do uruchamiania lokalnie. Wysyłka e-maili i logowanie przez zewnętrznych dostawców nie są skonfigurowane. W środowisku Development potwierdzenie adresu e-mail nie jest wymagane.
