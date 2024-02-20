using Microsoft.EntityFrameworkCore;

namespace PPE.Data.Services;

public class ErrorHandlingService
{
    public string GetFullErrorMessage(DbUpdateException? ex)
    {
        var messages = new List<string>();
        Exception? currentException = ex;

        while (currentException != null)
        {
            messages.Add(currentException.Message);
            currentException = currentException.InnerException;
        }

        return string.Join(" ", messages);
    }
}