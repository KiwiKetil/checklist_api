namespace Checklist_API.Features.ExceptionHandling;

public static class CustomExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
            : base($"User already exists")
        {
        }
    }
}
