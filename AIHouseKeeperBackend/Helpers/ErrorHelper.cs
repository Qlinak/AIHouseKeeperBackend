namespace AIHouseKeeperBackend.Helpers;

public static class ErrorHelper
{
    public static string FormatErrorMessage(string functionName, string ex)
    {
        return $"Encountered error in {functionName}, error: {ex}, at: {DateTime.UtcNow}";
    }
}