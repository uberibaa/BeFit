using Microsoft.AspNetCore.Identity;

namespace BeFit.Data;

public sealed class PolishIdentityErrors : IdentityErrorDescriber
{
    private static IdentityError Error(string code, string message) =>
        new() { Code = code, Description = message };

    public override IdentityError DuplicateUserName(string userName) =>
        Error(nameof(DuplicateUserName), "Konto o podanym adresie już istnieje.");
    public override IdentityError DuplicateEmail(string email) =>
        Error(nameof(DuplicateEmail), "Ten adres e-mail jest już używany.");
    public override IdentityError InvalidEmail(string? email) =>
        Error(nameof(InvalidEmail), "Podaj poprawny adres e-mail.");
    public override IdentityError InvalidUserName(string? userName) =>
        Error(nameof(InvalidUserName), "Nieprawidłowa nazwa użytkownika.");
    public override IdentityError PasswordMismatch() =>
        Error(nameof(PasswordMismatch), "Nieprawidłowe hasło.");
    public override IdentityError PasswordTooShort(int length) =>
        Error(nameof(PasswordTooShort), $"Hasło musi mieć co najmniej {length} znaków.");
    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        Error(nameof(PasswordRequiresNonAlphanumeric), "Hasło musi zawierać znak specjalny.");
    public override IdentityError PasswordRequiresDigit() =>
        Error(nameof(PasswordRequiresDigit), "Hasło musi zawierać cyfrę.");
    public override IdentityError PasswordRequiresLower() =>
        Error(nameof(PasswordRequiresLower), "Hasło musi zawierać małą literę.");
    public override IdentityError PasswordRequiresUpper() =>
        Error(nameof(PasswordRequiresUpper), "Hasło musi zawierać wielką literę.");
    public override IdentityError InvalidToken() =>
        Error(nameof(InvalidToken), "Nieprawidłowy lub wygasły token.");
}
