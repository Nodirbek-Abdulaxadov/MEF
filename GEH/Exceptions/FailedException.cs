namespace GEH.Exceptions;

public class FailedException(string errorMessage = "Something went wrong")
    : Exception(errorMessage)
{ }