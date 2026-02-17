namespace AcmeCorporation.Core.Validators;

public class AgeValidator
{
    public static bool IsAdult(DateTime dateOfBirth)
    {
        // Age is calculated based on
        // the current date - date of birth
        
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))  age--;
        return age >= 18;
    }
}