namespace Checklist_API.Extensions;

public static class CustomExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string email)
            : base($"User already exists with email: {email}")
        {
        }
    }
}
